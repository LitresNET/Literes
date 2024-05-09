using System.Security.Claims;
using Hangfire;
using Litres.Application.Extensions;
using Litres.Application.Middlewares;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Логирование на уровне приложения
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration["Database:ConnectionString"]));

builder.Services.AddIdentity<User, IdentityRole<long>>(options =>
    options.User.RequireUniqueEmail = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddConfiguredSerilog(builder.Configuration)
    .AddRouting(opt => opt.LowercaseUrls = true)
    .AddRepositories()
    .AddServices()
    .AddMiddlewares()
    .AddAuthorization()
    .AddConfiguredHangfire(builder.Configuration)
    .AddConfiguredAuthentication(builder.Configuration)
    .AddConfiguredAutoMapper()
    .AddEndpointsApiExplorer()
    .AddConfiguredSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddCors(options => options.AddDefaultPolicy(policyBuilder => 
    policyBuilder
        .WithOrigins(builder.Configuration["CorsPolicy:Origin"]!)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()));

var application = builder.Build();

if (application.Environment.IsDevelopment())
{
    application
        .UseSwagger()
        .UseSwaggerUI();
}

// For create roles
using (var scope = application.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();
    var roles = new[] { "Admin", "Publisher", "Member" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<long>(role));
            await roleManager.AddClaimAsync((await roleManager.FindByNameAsync(role))!,
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
        }
    }
}

application
    .UseCors()
    .UseMiddleware<ExceptionMiddleware>()
    .UseAuthentication()
    .UseAuthorization()
    .UseHttpsRedirection()
    .UseHangfireDashboard();

RecurringJob.AddOrUpdate<ISubscriptionCheckerService>("checkSubscriptions", service => service.CheckUsersSubscriptionExpirationDate(), "0 6 * * *");

application.MapControllers();

application.Run();
