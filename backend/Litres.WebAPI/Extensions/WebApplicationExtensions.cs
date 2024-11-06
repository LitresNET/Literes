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
        var roles = new[] { "Admin", "Publisher", "Member", "Agent" };

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
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();
        
        try
        {
            logger.LogInformation("Applying migrations...");
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation("There are pending migrations. Applying them now...");
                await context.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("No pending migrations found. Database is up to date.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying migrations.");
            throw;
        }
    }
}