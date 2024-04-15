using Hangfire;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Repositories;
using Litres.Main.Middlewares;
using Litres.Main.Services;

namespace Litres.Main.Extensions;

public static class WebApplicationExtension
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        #region Middlewares
        builder.Services.AddScoped<ExceptionMiddleware>();
        #endregion
        
        #region Services
        builder.Services.AddScoped<IRequestService, RequestService>();
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IRegistrationService, UserService>();
        builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
        builder.Services.AddScoped<ISubscriptionCheckerService, SubscriptionCheckerService>();
        #endregion
        
        #region Repositories
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion
    }
}