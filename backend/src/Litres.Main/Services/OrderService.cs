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
        // TODO: из-за переноса выброса исключений при не найдённых сущностях в абстрактный класс Repository<T>...
        // ...и отсутствии валидации в контроллере появились вот такие непонятные вызовы.
        // Чтобы от них избавиться надо валидацию делать в контроллере.
        _ = await unitOfWork.GetRepository<User>().GetByIdAsync(order.UserId);
        _ = await unitOfWork.GetRepository<PickupPoint>().GetByIdAsync(order.PickupPointId);
        
        order.Books = new List<Book>();
        foreach (var orderBook in order.OrderedBooks)
        {
            var book = await unitOfWork.GetRepository<Book>().GetByIdAsync(orderBook.BookId);
            if (book.Count < orderBook.Quantity)
                throw new BusinessException("More books have been requested than are left in stock");
            
            order.Books.Add(book);
        }

        order = await unitOfWork.GetRepository<Order>().AddAsync(order);
        await unitOfWork.SaveChangesAsync();
        
        return order;
    }

    public async Task<Order> GetOrderInfo(long orderId)
    {
        var orderRepository = (IOrderRepository)unitOfWork.GetRepository<Order>();
        var order = await orderRepository.GetWithFilterAsync(
            x => x.Id == orderId,
            new List<Expression<Func<Order, object>>> {y => y.OrderedBooks});
        
        return order;
    }

    public async Task<Order> ConfirmOrderAsync(long orderId, bool isSuccess)
    {
        var order = await unitOfWork.GetRepository<Order>().GetByIdAsync(orderId);
        order.IsPaid = isSuccess;
        order = unitOfWork.GetRepository<Order>().Update(order);
        await unitOfWork.SaveChangesAsync();

        return order;
    }
}