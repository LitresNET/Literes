using System.Linq.Expressions;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;

namespace Litres.Main.Services;

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    public async Task<Order> CreateOrderAsync(Order order)
    {
        var user = await unitOfWork.GetRepository<User>().GetByIdAsync(order.UserId);
        var pickUpPoint = await unitOfWork.GetRepository<PickupPoint>().GetByIdAsync(order.PickupPointId);

        if (user is null)
        {
            throw new EntityNotFoundException(typeof(User), order.UserId.ToString());
        }
        if (pickUpPoint is null)
        {
            throw new EntityNotFoundException(typeof(PickupPoint), order.PickupPointId.ToString());
        }
        
        order.Books = new List<Book>();
        foreach (var orderBook in order.OrderedBooks)
        {
            var book = await unitOfWork.GetRepository<Book>().GetByIdAsync(orderBook.BookId);
            if (book is null || book.Count < orderBook.Quantity)
            {
                // TODO: выбрасывать ошибку об отсутствии товаров
                return null;
            }
            order.Books.Add(book);
        }

        await unitOfWork.GetRepository<Order>().AddAsync(order);
        await unitOfWork.SaveChangesAsync();
        
        return order;
    }

    public async Task<Order> GetOrderInfo(long orderId)
    {
        var orderRepository = (IOrderRepository)unitOfWork.GetRepository<Order>();
        var order = await orderRepository.GetAsync(
            x => x.Id == orderId, 
            [ y => y.OrderedBooks ]);
            
        if (order is null)
        {
            throw new EntityNotFoundException(typeof(Order), orderId.ToString());
        }

        return order;
    }

    public async Task ConfirmOrderAsync(long orderId, bool isSuccess)
    {
        var order = await unitOfWork.GetRepository<Order>().GetByIdAsync(orderId);
        if (order is null)
            throw new EntityNotFoundException(typeof(Order), orderId.ToString());

        order.IsPaid = isSuccess;
        unitOfWork.GetRepository<Order>().Update(order);
        
        await unitOfWork.SaveChangesAsync();
    }
}