using System.Linq.Expressions;
using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;

namespace Litres.Application.Services;

public class OrderService(
    IUserRepository userRepository,
    IPickupPointRepository pickupPointRepository,
    IBookRepository bookRepository,
    IOrderRepository orderRepository) : IOrderService
{
    public async Task<Order> CreateOrderAsync(Order order)
    {
        
        _ = await userRepository.GetByIdAsync(order.UserId);
        _ = await pickupPointRepository.GetByIdAsync(order.PickupPointId);
        
        order.Books = new List<Book>();
        foreach (var orderBook in order.OrderedBooks)
        {
            var book = await bookRepository.GetByIdAsync(orderBook.BookId);
            if (book.Count < orderBook.Quantity)
                throw new BusinessException("More books have been requested than are left in stock");
            
            order.Books.Add(book);
        }

        order = await orderRepository.AddAsync(order);
        await orderRepository.SaveChangesAsync();
        
        return order;
    }

    public async Task<Order> GetOrderInfo(long orderId)
    {
        var order = await orderRepository.GetWithFilterAsync(
            x => x.Id == orderId,
            new List<Expression<Func<Order, object>>> {y => y.OrderedBooks});
        
        return order;
    }

    public async Task<Order> ConfirmOrderAsync(long orderId, bool isSuccess)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        order.IsPaid = isSuccess;
        order = orderRepository.Update(order);
        await orderRepository.SaveChangesAsync();

        return order;
    }
}