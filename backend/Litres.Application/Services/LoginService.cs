using System.Security.Claims;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Litres.Application.Services;

public class LoginService(
    UserManager<User> userManager,
    RoleManager<IdentityRole<long>> roleManager,
    IJwtTokenService jwtTokenService) : ILoginService
{
    public async Task<string> LoginUserFromExternalServiceAsync(string email, IEnumerable<Claim>? externalClaims = null)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new User
            {
                RoleName = "Member",
                Email = email,
                Name = externalClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "",
                IsAdditionalRegistrationRequired = true,
                UserName = Guid.NewGuid().ToString().Replace("-", "").ToLower()
            };
            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
                return "";
        }
        
        var claims = jwtTokenService.CreateClaimsByUser(user);
        claims.AddRange(externalClaims ?? Array.Empty<Claim>());
        
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            var identityRole = await roleManager.FindByNameAsync(role);
            claims.AddRange(await roleManager.GetClaimsAsync(identityRole!));
        }
        
        var token = jwtTokenService.CreateJwtToken(claims);
        
        return token;
    }
}