using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Litres.Domain.Exceptions;

namespace Litres.Application.Services;

public class OrderService(
    INotificationService notificationService,
    IPickupPointRepository pickupPointRepository,
    IBookRepository bookRepository,
    IOrderRepository orderRepository) : IOrderService
{
    public async Task<Order> GetOrderByIdAsNoTrackingAsync(long orderId)
    {
        return await orderRepository.GetByIdAsNoTrackingAsync(orderId);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        await pickupPointRepository.GetByIdAsNoTrackingAsync(order.PickupPointId);
        
        order.Books = new List<Book>();
        foreach (var orderBook in order.OrderedBooks)
        {
            var book = await bookRepository.GetByIdAsync(orderBook.BookId);
            if (book.Count < orderBook.Quantity)
                throw new BusinessException("More books have been requested than are left in stock");
            
            order.Books.Add(book);
        }

        var dbOrder = await orderRepository.AddAsync(order);
        await orderRepository.SaveChangesAsync();
        
        await notificationService.NotifyOrderStatusChange(dbOrder);
        
        return dbOrder;
    }
    
    public async Task<Order> UpdateOrderAsync(Order order)
    {
        var dbOrder = await orderRepository.GetByIdAsync(order.Id);
        if (dbOrder.Status >= OrderStatus.Assembly)
            throw new InvalidOperationException("Order already in immutable state!");

        dbOrder.OrderedBooks = order.OrderedBooks;
        dbOrder.PickupPointId = order.PickupPointId;
        var updatedOrder = orderRepository.Update(dbOrder);

        await notificationService.NotifyOrderStatusChange(updatedOrder);
        
        await orderRepository.SaveChangesAsync();
        
        return order;
    }

    public async Task<Order> UpdateOrderStatusAsync(long orderId, OrderStatus status)
    {
        var dbOrder = await orderRepository.GetByIdAsync(orderId);
        dbOrder.Status = status;
        dbOrder = orderRepository.Update(dbOrder);
        await orderRepository.SaveChangesAsync();

        await notificationService.NotifyOrderStatusChange(dbOrder);
        
        return dbOrder;
    }

    public async Task<Order> DeleteOrderByIdAsync(long orderId)
    {
        var dbOrder = await orderRepository.GetByIdAsync(orderId);
        orderRepository.Delete(dbOrder);
        await orderRepository.SaveChangesAsync();
        return dbOrder;
    }

    public async Task<decimal> TryPayOrderAsync(long orderId)
    {
        var dbOrder = await orderRepository.GetByIdAsync(orderId);
        if (dbOrder.Status > OrderStatus.Paid)
            throw new BusinessException("Order already paid!");
        
        // достаём пользователя из заказа -> если денег не хватает возвращаем разницу,
        // иначе списываем деньги и возвращаем 0.
        var user = dbOrder.User;
        var totalOrderPrice = dbOrder.OrderedBooks.Sum(b => b.Quantity * b.Book.Price);
        if (user.Wallet < totalOrderPrice)
            return totalOrderPrice - user.Wallet;

        user.Wallet -= totalOrderPrice;
        dbOrder.Status = OrderStatus.Paid;
        orderRepository.Update(dbOrder);
        await orderRepository.SaveChangesAsync();
        
        await notificationService.NotifyOrderStatusChange(dbOrder);

        return 0M;
    }
}