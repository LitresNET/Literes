using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Orders;

//TODO: "Распаковать" Dto на поля
public record UpdateOrderCommand(OrderDto OrderDto) : ICommand<OrderDto>;