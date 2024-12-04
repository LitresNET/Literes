using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Litres.Application.Commands.SignIn.Handlers;

//TODO: возможно нужен реврайт, слишком много всего намешано
public class SignInUserCommandHandler(UserManager<User> userManager,
    SignInManager<User> signInManager,
    RoleManager<IdentityRole<long>> roleManager,
    IJwtTokenService jwtTokenService) : ICommandHandler<SignInUserCommand, string>
{
    public async Task<string> HandleAsync(SignInUserCommand command)
    {
        var user = await userManager.FindByEmailAsync(command.Email) ??
                   throw new EntityNotFoundException(typeof(User), command.Email);

        if (user.IsAdditionalRegistrationRequired)
            throw new AdditionalRegistrationRequiredException("Only OAuth is permitted.");
        
        var result = await signInManager.CheckPasswordSignInAsync(user, command.Password, false);
        
        if (result == SignInResult.Failed)
            throw new PasswordNotMatchException();
        
        var claims = jwtTokenService.CreateClaimsByUser(user);
        
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            var identityRole = await roleManager.FindByNameAsync(role);
            claims.AddRange(await roleManager.GetClaimsAsync(identityRole!));
        }

        await signInManager.SignInAsync(user, true);
        return jwtTokenService.CreateJwtToken(claims);
    }
}