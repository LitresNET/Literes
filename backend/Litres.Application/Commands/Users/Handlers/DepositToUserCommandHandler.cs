using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Users.Handlers;

public class DepositToUserCommandHandler(IUserRepository userRepository) : ICommandHandler<DepositToUserCommand>
{
    public async Task HandleAsync(DepositToUserCommand command)
    {
        var dbUser = await userRepository.GetByIdAsync(command.UserId);
        dbUser.Wallet += command.Amount;
        //TODO: По-моему, вызывать Update необязательно. Предыдущая строчка и так добавила в кошелёк деняк
        userRepository.Update(dbUser);
        await userRepository.SaveChangesAsync();;
    }
}