using AutoMapper;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Orders.Handlers;

public class UpdateOrderStatusCommandHandler(
    IOrderRepository orderRepository,
    INotificationService notificationService,
    IMapper mapper
) : ICommandHandler<UpdateOrderStatusCommand, OrderDto>
{
    public async Task<OrderDto> HandleAsync(UpdateOrderStatusCommand command)
    {
        var dbOrder = await orderRepository.GetByIdAsync(command.OrderId);

        dbOrder.Status = command.Status;
        orderRepository.Update(dbOrder);

        await orderRepository.SaveChangesAsync();
        await notificationService.NotifyOrderStatusChange(dbOrder);

        return mapper.Map<OrderDto>(dbOrder);
    }
}