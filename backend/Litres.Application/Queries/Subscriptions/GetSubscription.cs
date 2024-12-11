using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Queries;

namespace Litres.Application.Queries.Subscriptions;

public record GetSubscription(long SubscriptionId) : IQuery<SubscriptionResponseDto>;