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
        var order = mapper.Map<Order>(command.OrderDto);
        var dbOrder = await orderRepository.GetByIdAsync(order.Id);

        if (dbOrder.Status >= OrderStatus.Assembly)
            throw new InvalidOperationException("Order already in immutable state!");

        dbOrder.OrderedBooks = order.OrderedBooks;
        dbOrder.PickupPointId = order.PickupPointId;

        var updatedOrder = orderRepository.Update(dbOrder);
        await notificationService.NotifyOrderStatusChange(updatedOrder);

        await orderRepository.SaveChangesAsync();

        return mapper.Map<OrderDto>(dbOrder);
    }
}