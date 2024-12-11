using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Enums;

namespace Litres.Application.Commands.Subscriptions.Handlers;

public class SubscriptionResetCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<SubscriptionResetCommand>
{
    public async Task HandleAsync(SubscriptionResetCommand command)
    {
        var dbUser = await userRepository.GetByIdAsync(command.UserId);

        if (dbUser.SubscriptionId is 1L) return;

        dbUser.SubscriptionId = (long)SubscriptionType.Free;
        dbUser.SubscriptionActiveUntil = DateTime.Now.Add(TimeSpan.FromDays(30));
        userRepository.Update(dbUser);

        await unitOfWork.SaveChangesAsync();
    }
}