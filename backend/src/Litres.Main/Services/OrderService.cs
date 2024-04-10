using System.ComponentModel.DataAnnotations;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;

namespace Litres.Main.Services;

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
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
        
        return await unitOfWork.GetRepository<Order>().AddAsync(order);
    }

    public async Task<Order> DeleteAsync(long orderId)
    {
        var orderRepository = unitOfWork.GetRepository<Order>();

        var dbOrder = await orderRepository.GetByIdAsync(orderId);
        if (dbOrder is null)
            throw new EntityNotFoundException(typeof(Order), orderId.ToString());

        return orderRepository.Delete(dbOrder);
    }
}