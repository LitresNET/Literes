using AutoMapper;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;


namespace Litres.Application.Commands.Orders.Handlers;

public class DeleteOrderCommandHandler(
    IOrderRepository orderRepository,
    IMapper mapper
) : ICommandHandler<DeleteOrderCommand, OrderDto>
{
    public async Task<OrderDto> HandleAsync(DeleteOrderCommand command)
    {
        var dbOrder = await orderRepository.GetByIdAsync(command.OrderId);
        orderRepository.Delete(dbOrder);
        await orderRepository.SaveChangesAsync();

        return mapper.Map<OrderDto>(dbOrder);
    }
}