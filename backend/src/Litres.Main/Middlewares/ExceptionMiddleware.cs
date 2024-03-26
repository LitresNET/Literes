using System.Text.Json;
using Litres.Main.Exceptions;

namespace Litres.Main.Middlewares;

public class ExceptionMiddleware(IWebHostEnvironment webHostEnvironment) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            if (context.Response.HasStarted)
                throw;

            var statusCode = 500;
            if (e is StorageUnavailableException)
                statusCode = 503;

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Message = e.Message,
                Exception = SerializeException(e)
            };
                
            var body = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(body);
        }
    }
    
    private string? SerializeException(Exception e)
    {
        // мы не включаем информацию об exception в продакшн из-за возможной утечки чувствительных данных
        return webHostEnvironment.IsProduction() ? null : e.ToString();
    }
}