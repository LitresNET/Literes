using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Litres.Application.Commands.SignUp.Handlers;

public class FinalizeUserCommandHandler(UserManager<User> userManager) : ICommandHandler<FinalizeUserCommand, IdentityResult>
{
    public async Task<IdentityResult> HandleAsync(FinalizeUserCommand command)
    {
        var dbUser = await userManager.FindByEmailAsync(command.Email);
        if (dbUser is null) return IdentityResult.Failed(new IdentityError {Description = "No such user found"});
        var result = await userManager.AddPasswordAsync(dbUser, command.Password);
        return result;
    }
}