using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IOrderService
{
    public Task<Order> CreateOrderAsync(Order order);
    public Task<Order> GetOrderInfo(long orderId);
    public Task<Order> ConfirmOrderAsync(long orderId, bool isSuccess);
}
