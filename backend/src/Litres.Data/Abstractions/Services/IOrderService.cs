using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IOrderService
{
    public Task<Order> GetByIdAsync(long orderId);
    public Task<Order> CreateAsync(Order order);
    public Task<Order> DeleteAsync(long orderId);
    public Task<Order> ChangeStatusAsync(long orderId, OrderStatus status);
}