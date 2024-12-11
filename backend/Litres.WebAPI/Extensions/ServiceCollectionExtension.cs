using System.Text;
using AutoMapper;
using Hangfire;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Commands.Books;
using Litres.Application.Commands.Books.Handlers;
using Litres.Application.Commands.SignIn;
using Litres.Application.Commands.SignIn.Handlers;
using Litres.Application.Commands.SignUp;
using Litres.Application.Commands.SignUp.Handlers;
using Litres.Application.Commands.Users;
using Litres.Application.Commands.Users.Handlers;
using Litres.Application.Consumers;
using Litres.Application.Dto;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Application.Extensions;
using Litres.Application.Hubs;
using Litres.Application.Queries.Books;
using Litres.Application.Queries.Orders;
using Litres.Application.Queries.Subscriptions;
using Litres.Application.Queries.Users;
using Litres.Application.Services;
using Litres.Application.Services.Options;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Abstractions.Services;
using Litres.Infrastructure.QueryHandlers.Books;
using Litres.Infrastructure.QueryHandlers.Orders;
using Litres.Infrastructure.QueryHandlers.Subscriptions;
using Litres.Infrastructure.QueryHandlers.Users;
using Litres.Infrastructure.Repositories;
using Litres.WebAPI.Configuration.Mapper;
using Litres.WebAPI.Controllers.Options;
using Litres.WebAPI.Middlewares;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

namespace Litres.WebAPI.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddScoped<ExceptionMiddleware>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IMemoryCache, MemoryCache>();
        services.AddScoped<NotificationHub>();
        
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IRequestService, RequestService>();
        services.AddScoped<ISubscriptionCheckerService, SubscriptionCheckerService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
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
            .UseSqlServerStorage(configuration["Database:Hangfire"]));
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
                    new UserMapperProfile(),
                    new ChatMapperProfile()
                }
            );
        });

        return services;
    }

    public static IServiceCollection AddConfiguredMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<MessageConsumer>();
            
            busConfigurator.UsingInMemory((context, config) => config.ConfigureEndpoints(context));
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
                options.IncludeErrorDetails = true;
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
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Authentication failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine("Authentication challenge triggered.");
                        return Task.CompletedTask;
                    }
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
                .Enrich.FromLogContext(), 
            preserveStaticLogger: true
        );

        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IHostEnvironment environment,
        IConfiguration configuration)
    {
        services.Configure<OrderControllerOptions>(configuration.GetSection("ExternalServices"));
        services.Configure<SubscriptionControllerOptions>(configuration.GetSection("ExternalServices"));
        services.Configure<JwtAuthenticationOptions>(configuration.GetSection("Authentication:Jwt"));
        services.Configure<GoogleAuthenticationOptions>(configuration.GetSection("Authentication:Google"));

        return services;
    }
    //TODO:Сделать автоматическую регистрацию
    public static IServiceCollection ConfigureCommands(this IServiceCollection services)
    {
        //можно зарегистрировать диспетчеры как Singleton, и так даже правильнее
        //но мы не можем из singleton-объекта обращаться к scoped-объекту
        //так что мы либо регистрируем диспетчеры как scoped, либо внутри диспетчера создаем внутренний scope
        //пример создания внутреннего scope оставила в классе CommandDispatcher
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<ICommandHandler<CreateBookCommand, RequestResponseDto>, CreateBookCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateBookCommand, RequestResponseDto>, UpdateBookCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteBookCommand, RequestResponseDto>, DeleteBookCommandHandler>();
        services.AddScoped<ICommandHandler<ChangeUserDataCommand, UserSettingsDto>, ChangeUserDataCommandHandler>();
        services.AddScoped<ICommandHandler<SignInUserCommand, string>, SignInUserCommandHandler>();
        services.AddScoped<ICommandHandler<SignUpUserCommand, IdentityResult>, SignUpUserCommandHandler>();
        services.AddScoped<ICommandHandler<FinalizeUserCommand, IdentityResult>, FinalizeUserCommandHandler>();
        services.AddScoped<ICommandHandler<DepositToUserCommand>, DepositToUserCommandHandler>();
        services.AddScoped<ICommandHandler<AddOrDeleteBookToUserFavouritesCommand>, AddOrDeleteBookToUserFavouritesCommandHandler>();
        
        return services;
    }
    
    public static IServiceCollection ConfigureQueries(this IServiceCollection services)
    {
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        services.AddScoped<IQueryHandler<GetBook, BookResponseDto>, GetBookQueryHandler>();
        services.AddScoped<IQueryHandler<GetBookCatalog, List<BookResponseDto>>, GetBookCatalogQueryHandler>();
        services.AddScoped<IQueryHandler<GetOrder, OrderDto>, GetOrderQueryHandler>();
        services.AddScoped<IQueryHandler<GetSubscription, SubscriptionResponseDto>, GetSubscriptionQueryHandler>();
        services.AddScoped<IQueryHandler<GetUserPublicData, UserPublicDataDto>, GetUserPublicDataQueryHandler>();
        services.AddScoped<IQueryHandler<GetOrderList, IEnumerable<OrderDto>>, GetOrderListQueryHandler>();
        services.AddScoped<IQueryHandler<GetUserPrivateData, UserPrivateDataDto>, GetUserPrivateDataQueryHandler>();
        services.AddScoped<IQueryHandler<GetPublisherData, PublisherStatisticsDto>, GetPublisherDataQueryHandler>();
        
        return services;
    }
}