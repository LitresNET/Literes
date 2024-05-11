using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.Domain.Abstractions.Services;

public interface IOrderService
{
    public Task<Order> CreateOrderAsync(Order order);
    public Task<Order> GetOrderByIdWithIncludes(long orderId);
    public Task<Order> ConfirmOrderAsync(long orderId, bool isSuccess);
    public Task<Order> ChangeOrderStatusAsync(long orderId, OrderStatus status);
}
