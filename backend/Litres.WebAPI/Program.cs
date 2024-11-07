using Hangfire;
using Litres.Application.Hubs;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Infrastructure;
using Litres.WebAPI.Extensions;
using Litres.WebAPI.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Логирование на уровне приложения
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", true, true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration["DB_CONNECTION_STRING"]));

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
    .AddConfiguredMassTransit()
    .AddEndpointsApiExplorer()
    .AddConfiguredSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddCors(options => options.AddDefaultPolicy(policyBuilder =>
{
        var origins = builder.Configuration.GetSection("CorsPolicy:Origins").Get<string[]>()!;
        policyBuilder
                .WithOrigins(origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
}));

builder.Services.ConfigureServices(builder.Environment, builder.Configuration);

var application = builder.Build();

await application.AddIdentityRoles();

if (application.Environment.IsDevelopment())
{
    application
        .UseSwagger()
        .UseSwaggerUI();
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
application.MapHub<NotificationHub>("api/hubs/notification");
application.MapHub<ChatHub>("api/hubs/chat");

application.Run();

// с настройками по умолчанию интеграционные тесты не видят namespace нашего Progrnam.cs - делаем публичным
public partial class Program;