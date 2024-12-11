using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Enums;
using Litres.Domain.Exceptions;

namespace Litres.Application.Commands.Orders.Handlers;

public class TryPayOrderCommandHandler(
    IOrderRepository orderRepository,
    INotificationService notificationService
) : ICommandHandler<TryPayOrderCommand, decimal>
{
    public async Task<decimal> HandleAsync(TryPayOrderCommand command)
    {
        var dbOrder = await orderRepository.GetByIdAsync(command.OrderId);

        if (dbOrder.Status > OrderStatus.Paid)
            throw new BusinessException("Order already paid!");

        var user = dbOrder.User;
        var totalOrderPrice = dbOrder.OrderedBooks.Sum(b => b.Quantity * b.Book.Price);

        if (user.Wallet < totalOrderPrice)
            return totalOrderPrice - user.Wallet;

        user.Wallet -= totalOrderPrice;
        dbOrder.Status = OrderStatus.Paid;

        orderRepository.Update(dbOrder);
        await orderRepository.SaveChangesAsync();
        await notificationService.NotifyOrderStatusChange(dbOrder);

        return 0M;
    }
}