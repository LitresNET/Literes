﻿using System.Text.Json;
using Litres.Data.Models;
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

            var statusCode = e switch
            {
                EntityNotFoundException => 404,
                EntityValidationFailedException => 422,
                PasswordNotMatchException => 400,
                StorageUnavailableException => 503,
                PermissionDeniedException => 403,
                _ => 500
            };

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