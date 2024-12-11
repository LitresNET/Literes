using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Subscriptions;

public record SubscriptionResetCommand(long UserId) : ICommand;