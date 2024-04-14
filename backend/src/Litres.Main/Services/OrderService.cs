using System.ComponentModel.DataAnnotations;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;

namespace Litres.Main.Services;

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    public Task<Order> GetByIdAsync(long orderId)
    {
        throw new NotImplementedException();
    }

    public async Task<Order> CreateAsync(Order order)
    {
        var context = new ValidationContext(order);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(order, context, results))
            throw new EntityValidationFailedException(typeof(Order), results);
        
        if (await unitOfWork.GetRepository<PickupPoint>().GetByIdAsync(order.PickupPointId) is null)
            throw new EntityNotFoundException(typeof(PickupPoint), order.PickupPointId.ToString());

        if (await unitOfWork.GetRepository<User>().GetByIdAsync(order.UserId) is null)
            throw new EntityNotFoundException(typeof(User), order.UserId.ToString());
        
        await unitOfWork.GetRepository<Order>().AddAsync(order);
        await unitOfWork.SaveChangesAsync();
        return order;
    }

    public async Task<Order> DeleteAsync(long orderId)
    {
        var orderRepository = unitOfWork.GetRepository<Order>();

        var dbOrder = await orderRepository.GetByIdAsync(orderId);
        if (dbOrder is null)
            throw new EntityNotFoundException(typeof(Order), orderId.ToString());
        
        orderRepository.Delete(dbOrder);
        await unitOfWork.SaveChangesAsync();
        return dbOrder;
    }

    public async Task<Order> ChangeStatusAsync(long orderId, OrderStatus status)
    {
        var orderRepository = unitOfWork.GetRepository<Order>();

        var dbOrder = await orderRepository.GetByIdAsync(orderId);
        if (dbOrder is null)
            throw new EntityNotFoundException(typeof(Order), orderId.ToString());

        if (dbOrder.Status > status)
            throw new InvalidOperationException("Can't change status of order to lower one.");
        
        dbOrder.Status = status;
        await unitOfWork.SaveChangesAsync();
        return dbOrder;
    }
}