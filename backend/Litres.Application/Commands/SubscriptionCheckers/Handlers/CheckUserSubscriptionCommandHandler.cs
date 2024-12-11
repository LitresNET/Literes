using Litres.Application.Abstractions.Repositories;
using Litres.Application.Commands.Subscriptions;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Services;

namespace Litres.Application.Commands.SubscriptionCheckers.Handlers;

public class CheckUserSubscriptionCommandHandler(
    IUserRepository userRepository,
    ICommandDispatcher commandDispatcher,
    IUnitOfWork unitOfWork
) : ICommandHandler<CheckUserSubscriptionCommand>
{
    public async Task HandleAsync(CheckUserSubscriptionCommand command)
    {
        var users = await userRepository.GetAllAsync();

        foreach (var u in users)
        {
            var commandRenew = new SubscriptionRenewCommand(u.Id);
            await commandDispatcher.DispatchAsync(commandRenew);
        }

        await unitOfWork.SaveChangesAsync();
    }
}