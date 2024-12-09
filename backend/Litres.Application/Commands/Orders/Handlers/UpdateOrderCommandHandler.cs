using AutoMapper;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.Application.Commands.Orders.Handlers;

public class UpdateOrderCommandHandler(
    IOrderRepository orderRepository,
    INotificationService notificationService,
    IMapper mapper
) : ICommandHandler<UpdateOrderCommand, OrderDto>
{
    public async Task<OrderDto> HandleAsync(UpdateOrderCommand command)
    {
        var dbOrder = await orderRepository.GetByIdAsync(command.Order.Id);

        if (dbOrder.Status >= OrderStatus.Assembly)
            throw new InvalidOperationException("Order already in immutable state!");

        dbOrder.OrderedBooks = command.Order.OrderedBooks;
        dbOrder.PickupPointId = command.Order.PickupPointId;

        var updatedOrder = orderRepository.Update(dbOrder);
        await notificationService.NotifyOrderStatusChange(updatedOrder);

        await orderRepository.SaveChangesAsync();

        return mapper.Map<OrderDto>(dbOrder);
    }
}