using Hangfire;
using Litres.Application.Hubs;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Infrastructure;
using Litres.WebAPI.Extensions;
using Litres.WebAPI.Middlewares;
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
    .AddJsonFile("appsettings.json", true, true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration["DB_CONNECTION_STRING"]));

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
{
        var origins = builder.Configuration.GetSection("CorsPolicy:Origins").Get<string[]>()!;
        policyBuilder
                .WithOrigins(origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
}));

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
    .UseMiddleware<ExceptionMiddleware>()
    .UseAuthentication()
    .UseAuthorization()
    .UseHttpsRedirection()
    .UseHangfireDashboard();

RecurringJob.AddOrUpdate<ISubscriptionCheckerService>("checkSubscriptions", service => service.CheckUsersSubscriptionExpirationDate(), "0 6 * * *");

application.MapControllers();
application.MapHub<NotificationHub>("api/hubs/notification");

application.Run();

// с настройками по умолчанию интеграционные тесты не видят namespace нашего Progrnam.cs - делаем публичным
public partial class Program { }

/*
DONE:   разбить на clean архитектуру (рефактор сомнительных штук которые на глаза попадутся)
        Решено: проект распилен, составлен список задач

DONE:   починить репозитории (проблемы с tracking)
        Решено: добавлен дополнительный метод для получения данных не отслеживая. Все методы, которые просто получают данные из бд теперь не отслеживаемые.

DONE:   из-за распилки проектов появилась непонятная проблема с парсингом seedConfig.
        Решено: добавлена загрузка сборки в ApplicationDbContext.

DONE:   починить метод фильтрации книг, EF Core не умеет в сложные предикаты
        Решено: добавлена библеотека LinqKit и теперь соответствующий репозиторий возвращает сразу список, а не IQueryable как было раньше

DONE:   починить добавление ролей (которое я удалил из Program.cs) гайды есть в группе
        Решено: вынесено в extension для WebApplication

DONE:   везде проставить доступ через атрибуты
        Решено: проставлен доступ к методам по ролям на своё усмотрение. Обнаружена нехватка методов в некоторых контроллерах, добавлено в задачи

DONE:   найти все IConfigurations в сервисах и заменить их на IOptions
        Решено: в extension методах замены не было, потому что мы всё это делаем всё равно на этапе запуска, там прирост скорости будет незначитальный

DONE:   финансовые операции должны списывать деньги с внутреннего счета, если не хватает, редирект на сервис оплаты
        Решено: метод обработки оплаты подписки был изменён. Теперь он возвращает число - сумма которой не хватает для оплаты

DONE:   добавить метод получения списка отзывов по книге (с пагинацией желательно)
        Решено: выполнено в соответствии и требованиями

DONE:   Signal R
        Решено: SignalR был добавлен для отправки уведомлений в реальном времени, появилась небольшая проблемка, идея для решения описана ниже

DONE:   (нет задачи)
        мелкие фиксы в работе с авторизацией и ревью.

DONE:   В PublisherRepository используется override на метод, хотя логика подразумевает немного другое - заменить
        Решено: метод переименован, использования подменены.

DONE:   В UserRepository есть несколько устаревших методов. Нужно их удалить
        Решено: как оказалось, эти методы и так нигде не использовались, удаление было только в репозитории.

? DONE: проблема с производительностью. Из-за того что ApplicationDbContext создаётся при каждом запросе, то как оказалось и сиды прогоняются каждый раз.
        Решено: при выкатывании приложения на продакшн - прямо в реальный мир а не в ветку main, сиды будут удалены в любом случае => нет проблемы.
*/

/*
TODO: Защита от мошенников ([ValidateAntiForgeryToken]) - это какой-то пиздец, там надо и мидлварю и фильтр походу, потому что дефолтная реализация работает только для Razor паджес(

TODO: настроить ссылку на дефолтную аватарку (UserEntityConfiguration)

TODO: из-за переноса выброса исключений при не найдённых сущностях в абстрактный класс Repository<T> и отсутствии ...
валидации в контроллере появились вот такие непонятные вызовы (OrderService). Чтобы от них избавиться надо валидацию делать в контроллере.
TODO: по кол-ву репозиториев можно в BookService с уверенностью сказать что он явно выполняет больше работы чем должен...
Чтобы избавиться от этого нужно сделать валидацию в контроллере ИЛИ что ещё лучше до него с помощью атрибута [ApiController] -
проблема в том что если мы хотим использовать валидатор от DataAnnotations нужно написать собственный провайдер для валидаторов
(потому что под все модели, по факту подойдёт один и тот же валидатор)

?   TODO: реализовать логику дорегистрации - тяжёлая задача из-за майкрософт айдентити, пока не смог решить
?   TODO: настроить логгер
?   TODO: роли добавляются мега криво, чтобы добавить новую роль надо залезть в код, изменить массив - по любому можно сделать лучше
?   TODO: сделать проверку на то, что запрос был прислан с нашего сервиса оплаты в OrderController
?   TODO: подумать над переносом всех методов, которые возвращают реквесты, в request контроллер
?   TODO: нужно изменить списывание денег со счёта. Логика в том, чтобы сохранять не только пополнения, но и списывания => нужно таблицу транзакций и добавлять все записи о деньгах.
?   TODO: под уведомления можно написать фабрику
?   TODO: возникло повторение кода в хабе с уведомлениями и в сервисе, не очень круто(
?   TODO: не очень крутое взаимодействие с уведомлениям, можно реализовать паттерн подписчик-издатель, где сервис заказа будет издателем...
    ... подразумеваю использование колбек механизма. То есть мы в сервисе заказа будем вызывать какие-то функции, но
    знать этого не будем, а сервис уведомлений подпишется на эти события и уже сам разберётся что ему делать с данными
?   TODO: убрать всех павликов в интерфейсах (все поля там по дефолту паблик)

*/