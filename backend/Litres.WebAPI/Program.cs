using Hangfire;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Infrastructure;
using Litres.WebAPI.Extensions;
using Litres.WebAPI.Hubs;
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
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration["Database:ConnectionString"]));

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
    .AddEndpointsApiExplorer()
    .AddConfiguredSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddCors(options => options.AddDefaultPolicy(policyBuilder => 
    policyBuilder
        .WithOrigins(builder.Configuration["CorsPolicy:Origin"]!)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()));

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
    // .UseMiddleware<ExceptionMiddleware>()
    .UseAuthentication()
    .UseAuthorization()
    .UseHttpsRedirection()
    .UseHangfireDashboard();

RecurringJob.AddOrUpdate<ISubscriptionCheckerService>("checkSubscriptions", service => service.CheckUsersSubscriptionExpirationDate(), "0 6 * * *");

application.MapControllers();
application.MapHub<NotificationHub>("api/hubs/notification");

application.Run();

//? DONE: разбить на clean архитектуру (рефактор сомнительных штук которые на глаза попадутся) - Решено: составлен список задач
// DONE: починить репозитории (проблемы с tracking) - Решено: добавлен дополнительный метод для получения данных не отслеживая. Все методы, которые просто получают данные из бд теперь не отслеживаемые.
// DONE: из-за распилки проектов появилась непонятная проблема с парсингом seedConfig. - Решено: добавлена загрузка сборки в ApplicationDbContext.
// DONE: починить метод фильтрации книг, EF Core не умеет в сложные предикаты - Решено: добавлена библеотека LinqKit и теперь соответствующий репозиторий возвращает сразу список, а не IQueryable как было раньше
// DONE: починить добавление ролей (которое я удалил из Program.cs) гайды есть в группе - Решено: вынесено в extension для WebApplication
// DONE: везде проставить доступ через атрибуты - Решено: проставлен доступ к методам по ролям на своё усмотрение. Обнаружена нехватка методов в некоторых контроллерах, добавлено в задачи
// DONE: найти все IConfigurations в сервисах и заменить их на IOptions - Решено: в extension методах замены не было, потому что мы всё это делаем всё равно на этапе запуска, там прирост скорости будет незначитальный

// TODO: [IN PROGRESS] финансовые операции должны списывать деньги с внутреннего счета, если не хватает, редирект на сервис оплаты
// TODO: добавить метод получения списка отзывов по книге (с пагинацией желательно)
// TODO: Signal R 
// TODO: Защита от мошенников ([ValidateAntiForgeryToken]) - это какой-то пиздец, там надо и мидлварю и фильтр походу, потому что дефолтная реализация работает только для Razor паджес(

// TODO: настроить ссылку на дефолтную аватарку (UserEntityConfiguration)
// TODO: В PublisherRepository используется override на метод, хотя логика подразумевает немного другое - заменить
// TODO: В UserRepository есть несколько устаревших методов. Нужно их удалить
// TODO: проблема с производительностью. Из-за того что ApplicationDbContext создаётся при каждом запросе, то как оказалось и сиды прогоняются каждый раз.

// TODO: из-за переноса выброса исключений при не найдённых сущностях в абстрактный класс Repository<T> и отсутствии ...
// валидации в контроллере появились вот такие непонятные вызовы (OrderService). Чтобы от них избавиться надо валидацию делать в контроллере.
// TODO: по кол-ву репозиториев можно в BookService с уверенностью сказать что он явно выполняет больше работы чем должен...
// Чтобы избавиться от этого нужно сделать валидацию в контроллере ИЛИ что ещё лучше до него с помощью атрибута [ApiController] -
// проблема в том что если мы хотим использовать валидатор от DataAnnotations нужно написать собственный провайдер для валидаторов
// (потому что под все модели, по факту подойдёт один и тот же валидатор)

//? TODO: реализовать логику дорегистрации - тяжёлая задача, пока не смог решить
//? TODO: настроить логгер
//? TODO: роли добавляются мега криво, чтобы добавить новую роль надо залезть в код, изменить массив - по любому можно сделать лучше
//? TODO: сделать проверку на то, что запрос был прислан с нашего сервиса оплаты в OrderController
//? TODO: подумать над переносом всех методов, которые возвращают реквесты, в request контроллер
