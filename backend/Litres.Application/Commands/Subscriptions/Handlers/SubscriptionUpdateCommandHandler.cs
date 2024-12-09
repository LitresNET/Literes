using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Litres.Domain.Exceptions;

namespace Litres.Application.Commands.Subscriptions.Handlers;

public class SubscriptionUpdateCommandHandler(
    IUserRepository userRepository,
    ISubscriptionRepository subscriptionRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<SubscriptionUpdateCommand, decimal>
{
    public async Task<decimal> HandleAsync(SubscriptionUpdateCommand command)
    {
        var dbUser = await userRepository.GetByIdAsync(command.UserId);
        var currentSubscription = dbUser.Subscription;
        var newSubscription = command.newSubscription;

        if (newSubscription.Name != SubscriptionType.Custom.ToString() &&
            Enum.TryParse(typeof(SubscriptionType), newSubscription.Name, out var subscriptionType))
        {
            var dbSubscription = await subscriptionRepository.GetByTypeAsync((SubscriptionType)subscriptionType);
            newSubscription = dbSubscription ?? throw new EntityNotFoundException(typeof(Subscription), newSubscription.Name);
        }

        if (currentSubscription.Price > newSubscription.Price)
        {
            if (newSubscription.Name == SubscriptionType.Custom.ToString())
                await subscriptionRepository.AddAsync(newSubscription);
            dbUser.Subscription = newSubscription;
        }
        else
        {
            if (dbUser.Wallet < newSubscription.Price)
                return currentSubscription.Price;

            dbUser.Wallet -= newSubscription.Price;
            dbUser.Subscription = newSubscription;
            dbUser.SubscriptionActiveUntil = DateTime.Now.Add(TimeSpan.FromDays(30));
        }

        if (currentSubscription.Name == SubscriptionType.Custom.ToString() &&
            newSubscription.Name != SubscriptionType.Custom.ToString())
        {
            subscriptionRepository.Delete(currentSubscription);
        }

        await unitOfWork.SaveChangesAsync();
        return dbUser.Subscription.Price;
    }
}