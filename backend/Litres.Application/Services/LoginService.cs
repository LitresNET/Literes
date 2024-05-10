using System.Security.Claims;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
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
        
        var claims = CreateClaimsByUser(user);
        
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            var identityRole = await roleManager.FindByNameAsync(role);
            claims.AddRange(await roleManager.GetClaimsAsync(identityRole!));
        }

        return jwtTokenService.CreateJwtToken(claims);
    }
    
    public async Task<string> LoginUserFromExternalServiceAsync(string email, IEnumerable<Claim>? externalClaims = null)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new User
            {
                RoleName = "Regular",
                Email = email,
                Name = GetNormalizedUserName(externalClaims),
                IsAdditionalRegistrationRequired = true,
                UserName = Guid.NewGuid().ToString().Replace("-", "").ToLower(),
                PhoneNumber = "",
                PasswordHash = ""
            };
            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
                return "";
        }
        
        var claims = CreateClaimsByUser(user);
        claims.AddRange(externalClaims ?? Array.Empty<Claim>());
        
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            var identityRole = await roleManager.FindByNameAsync(role);
            claims.AddRange(await roleManager.GetClaimsAsync(identityRole!));
        }
        
        var token = jwtTokenService.CreateJwtToken(claims);
        
        return token;
    }

    private string GetNormalizedUserName(IEnumerable<Claim>? externalClaims)
    {
        var userName = externalClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "";
        var normalizedUserName = string.Join("", userName.Select(c => char.IsLetterOrDigit(c) ? c.ToString() : ""));
        return normalizedUserName is null or "" 
            ? "user123123" 
            : normalizedUserName;
    }

    private static List<Claim> CreateClaimsByUser(User user)
    {
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, user.Id.ToString()),
            new(CustomClaimTypes.SubscriptionTypeId, user.SubscriptionId.ToString()),
            new(CustomClaimTypes.SubscriptionActiveUntil, user.SubscriptionActiveUntil.ToShortDateString())
        };

        return claims;
    }
}