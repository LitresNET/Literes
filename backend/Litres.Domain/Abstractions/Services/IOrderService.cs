using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.Domain.Abstractions.Services;

public interface IOrderService
{
    public Task<Order> GetOrderByIdAsNoTrackingAsync(long orderId);
    public Task<Order> CreateOrderAsync(Order order);
    public Task<Order> UpdateOrderStatusAsync(long orderId, OrderStatus status);
    public Task<Order> UpdateOrderAsync(Order order);
}
