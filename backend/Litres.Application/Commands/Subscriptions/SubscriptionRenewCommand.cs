using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Subscriptions;

public record SubscriptionRenewCommand(long UserId) : ICommand<SubscriptionResponseDto>;