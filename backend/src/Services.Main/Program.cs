using backend.Abstractions;
using backend.Configurations;
using backend.Configurations.Mapping;
using backend.Middlewares;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration["Database:ConnectionString"])
);

builder.Services.AddAutoMapper(
    cfg => cfg.AddProfile<BookMapperProfile>()
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMiddleware, ExceptionMiddleware>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

app.Run();
