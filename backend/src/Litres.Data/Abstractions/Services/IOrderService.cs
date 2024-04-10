using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IOrderService
{
    public Task<Order> GetById(long orderId);
    public Task<Order> CreateAsync(Order order);
    public Task<Order> DeleteAsync(long orderId);
}