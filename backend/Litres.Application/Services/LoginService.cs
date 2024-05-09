using System.Security.Claims;
using Litres.Application.Models;
using Litres.Data.Exceptions;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Litres.Application.Services;

public class LoginService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    RoleManager<IdentityRole<long>> roleManager,
    IJwtTokenService jwtTokenService) : ILoginService
{
    public async Task<string> LoginUserAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            throw new EntityNotFoundException(typeof(User), email);
    
        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        
        if (result == SignInResult.Failed)
            throw new PasswordNotMatchException();
        
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, user.Id.ToString())
        };
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            var identityRole = await roleManager.FindByNameAsync(role);
            claims.AddRange(await roleManager.GetClaimsAsync(identityRole!));
        }
        
        claims.Add(new Claim(CustomClaimTypes.SubscriptionTypeId, user.SubscriptionId.ToString()!));
        claims.Add(new Claim(CustomClaimTypes.SubscriptionActiveUntil, user.SubscriptionActiveUntil.ToShortDateString()));

        return jwtTokenService.CreateJwtToken(claims);
    }
    
    public async Task<string> LoginUserFromExternalServiceAsync(string email, IEnumerable<Claim> externalClaims = null)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            throw new EntityNotFoundException(typeof(User), email);
        
        /* TODO: реализовать логику дорегистрации
        user = new User 
            { 
                Email = email, 
                Name = email.Split('@')[0], 
                UserName = email,
                PasswordHash = "123destroyMe!"
            };
        await userManager.CreateAsync(user);
        */
        
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, user.Id.ToString()),
            new(CustomClaimTypes.SubscriptionTypeId, user.SubscriptionId.ToString()),
            new(CustomClaimTypes.SubscriptionActiveUntil, user.SubscriptionActiveUntil.ToShortDateString())
        };
        
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            var identityRole = await roleManager.FindByNameAsync(role);
            claims.AddRange(await roleManager.GetClaimsAsync(identityRole!));
        }
        
        claims.AddRange(externalClaims);

        return jwtTokenService.CreateJwtToken(claims);
    }
}