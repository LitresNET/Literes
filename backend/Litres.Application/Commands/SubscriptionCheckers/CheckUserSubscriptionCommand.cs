using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.SubscriptionCheckers;

public record CheckUserSubscriptionCommand(long UserId) : ICommand;