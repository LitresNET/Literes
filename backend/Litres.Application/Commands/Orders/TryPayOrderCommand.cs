using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Orders;

public record TryPayOrderCommand(long OrderId) : ICommand<decimal>;