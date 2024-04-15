using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IOrderService
{
    public Task<Order> GetByIdAsync(long orderId);
    public Task<Order> DeleteAsync(long orderId);
    public Task<Order> ChangeStatusAsync(long orderId, OrderStatus status);
  
    public Task<Order> CreateOrderAsync(Order order);
    public Task<Order> GetOrderInfo(long orderId);
    public Task<Order> ConfirmOrderAsync(long orderId, bool isSuccess);
}
