using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Litres.WebAPI.Extensions;

public static class WebApplicationExtensions
{
    public static async Task AddIdentityRoles(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();
        var roles = new[] { "Admin", "Publisher", "Member" };

        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role)) continue;
            
            var identityRole = new IdentityRole<long>(role);
            await roleManager.CreateAsync(identityRole);
            await roleManager.AddClaimAsync(identityRole, new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
        }
    }
}