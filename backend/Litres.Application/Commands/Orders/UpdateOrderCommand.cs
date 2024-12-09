using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Orders;

public record UpdateOrderCommand(Order Order) : ICommand<OrderDto>;