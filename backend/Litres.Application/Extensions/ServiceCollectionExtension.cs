﻿using System.Text;
using AutoMapper;
using Hangfire;
using Litres.Application.Configuration.Mapper;
using Litres.Application.Controllers.Options;
using Litres.Application.Middlewares;
using Litres.Application.Services;
using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

namespace Litres.Application.Extensions;

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
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPickupPointRepository, PickupPointRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<ISeriesRepository, SeriesRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();

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
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:SecurityKey"]!)
                    ),
                    ValidateIssuerSigningKey = true,
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
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
                .Enrich.FromLogContext()
        );
        
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IHostEnvironment environment,
        IConfiguration configuration)
    {
        services.Configure<OrderControllerOptions>(configuration.GetSection("ExternalServices"));

        return services;
    }
}