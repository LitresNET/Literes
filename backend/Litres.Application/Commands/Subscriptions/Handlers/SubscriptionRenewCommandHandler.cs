using AutoMapper;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.Application.Commands.Subscriptions.Handlers;

public class SubscriptionRenewCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<SubscriptionRenewCommand, SubscriptionResponseDto>
{
    public async Task<SubscriptionResponseDto> HandleAsync(SubscriptionRenewCommand command)
    {
        var dbUser = await userRepository.GetByIdAsync(command.UserId);

        if (dbUser.Wallet < dbUser.Subscription.Price)
        {
            dbUser.SubscriptionId = (long) SubscriptionType.Free;
            dbUser.SubscriptionActiveUntil = DateTime.Now.Add(TimeSpan.FromDays(30));
            userRepository.Update(dbUser);
        }
        else
        {
            dbUser.Wallet -= dbUser.Subscription.Price;
            dbUser.SubscriptionActiveUntil += TimeSpan.FromDays(30);
        }

        await unitOfWork.SaveChangesAsync();
        return mapper.Map<SubscriptionResponseDto>(dbUser.Subscription);
    }
}