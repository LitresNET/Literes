using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.Application.Commands.Orders;

public record UpdateOrderStatusCommand(long OrderId, OrderStatus Status) : ICommand<OrderDto>;