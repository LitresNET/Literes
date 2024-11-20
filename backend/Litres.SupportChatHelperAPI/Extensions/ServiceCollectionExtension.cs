using Litres.SupportChatHelperAPI.Abstractions.Repositories;
using Litres.SupportChatHelperAPI.Consumers;
using Litres.SupportChatHelperAPI.Services.Repositories;
using MassTransit;
using Serilog;
using Serilog.Events;

namespace Litres.SupportChatHelperAPI.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMessageRepository, MessageRepository>();

        return services;
    }
    
    public static IServiceCollection AddConfiguredMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<MessageConsumer>();
            
            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", hc =>
                {
                    hc.Username("guest");
                    hc.Password("guest");
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
    
    public static IServiceCollection AddConfiguredSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((servs, loggerConfiguration) =>
                loggerConfiguration
                    .MinimumLevel.Verbose()
                    .MinimumLevel.Override("Microsoft.AspNetCore.Cors.Infrastructure", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                    .ReadFrom.Configuration(configuration)
                    .ReadFrom.Services(servs)
                    .Enrich.FromLogContext(), 
            
            preserveStaticLogger: true
        );

        return services;
    }
}