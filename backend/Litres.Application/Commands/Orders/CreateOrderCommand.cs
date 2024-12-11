using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Orders;

public record CreateOrderCommand(Order Order) : ICommand<OrderDto>;