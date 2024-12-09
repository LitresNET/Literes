using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Orders;

public record DeleteOrderCommand(long OrderId) : ICommand<OrderDto>;