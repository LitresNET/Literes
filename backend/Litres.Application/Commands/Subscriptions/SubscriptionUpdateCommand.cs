using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Subscriptions;

public record SubscriptionUpdateCommand(long UserId, Subscription newSubscription) : ICommand<decimal>;