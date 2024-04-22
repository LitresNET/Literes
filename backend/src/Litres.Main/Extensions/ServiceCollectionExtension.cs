using System.Text;
using AutoMapper;
using Hangfire;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Configurations.Mapping;
using Litres.Data.Repositories;
using Litres.Main.Middlewares;
using Litres.Main.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Litres.Main.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddScoped<ExceptionMiddleware>();

        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IRequestService, RequestService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<ISubscriptionCheckerService, SubscriptionCheckerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddConfiguredHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(opt => opt
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration["Database:HangfireConnectionString"]));
        services.AddHangfireServer();

        return services;
    }

    public static IServiceCollection AddConfiguredSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddConfiguredAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfiles( new List<Profile>
                {
                    new BookMapperProfile(),
                    new OrderMapperProfile(),
                    new PublisherMapperProfile(),
                    new RequestMapperProfile(),
                    new ReviewMapperProfile(),
                    new SubscriptionMapperProfile(),
                    new UserMapperProfile()
                }
            );
        });
        
        return services;
    }

    public static IServiceCollection AddConfiguredAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]!)
                    ),
                    ValidateIssuerSigningKey = true,
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
                options.CallbackPath = configuration["Authentication:Google:RedirectUri"]!;
            });

        return services;
    }
}