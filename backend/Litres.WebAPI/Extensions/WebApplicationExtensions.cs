using System.Security.Claims;
using Litres.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    
    public static async Task AddMigrations(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await context.Database.MigrateAsync();
    }
}