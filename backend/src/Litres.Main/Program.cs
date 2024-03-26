using System.Text;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using backend.Middlewares;
using Litres.Data.Models;
using Litres.Data.Repositories;
using backend.Services;
using Litres.Data.Configurations;
using Litres.Data.Configurations.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration["Database:ConnectionString"])
);
builder.Services.AddDefaultIdentity<User>(options =>
    options.User.RequireUniqueEmail = true).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthorization();
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"])
            ),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAutoMapper(cfg => 
    cfg.AddProfile<BookMapperProfile>());
builder.Services.AddAutoMapper(cfg =>
    cfg.AddProfile<UserMapperProfile>());


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: Исправить
// builder.Services.AddSingleton<IWebHostEnvironment, WebHostEnvironment>();
// builder.Services.AddSingleton<IMiddleware, ExceptionMiddleware>();

builder.Services.AddScoped<ExceptionMiddleware>();


builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
