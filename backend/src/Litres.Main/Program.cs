using System.Text;
using Hangfire;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Data.Configurations;
using Litres.Data.Configurations.Mapping;
using Litres.Main.Extensions;
using Litres.Main.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(opt => opt
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration["Database:HangfireConnectionString"]));
builder.Services.AddHangfireServer();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration["Database:ConnectionString"]));

builder.Services.AddDefaultIdentity<User>(options =>
    options.User.RequireUniqueEmail = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"]!)
            ),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<SubscriptionMapperProfile>();
    cfg.AddProfile<RequestMapperProfile>();
    cfg.AddProfile<UserMapperProfile>();
    cfg.AddProfile<BookMapperProfile>();
    cfg.AddProfile<OrderMapperProfile>();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddDependencies();

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
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<ISubscriptionCheckerService>("checkSubscriptions", service => service.CheckUsersSubscriptionExpirationDate(), "0 6 * * *");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
