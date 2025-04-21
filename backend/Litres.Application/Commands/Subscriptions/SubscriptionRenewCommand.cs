using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Subscriptions;

public record SubscriptionRenewCommand(long UserId) : ICommand<SubscriptionResponseDto>;