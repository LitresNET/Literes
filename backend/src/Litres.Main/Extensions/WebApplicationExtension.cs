using backend.Middlewares;
using backend.Services;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Repositories;

namespace backend.Extensions;

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
        #endregion
        
        #region Repositories
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IBookRepository, BookRepository>();
        builder.Services.AddScoped<IRequestRepository, RequestRepository>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
        builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
        builder.Services.AddScoped<IContractRepository, ContractRepository>();
        #endregion
    }
}