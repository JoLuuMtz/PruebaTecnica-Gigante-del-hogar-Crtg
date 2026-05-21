using BookingSystem.Shared.Exceptions;
using BookingSystem.Shared.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace BookingSystem.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error no controlado.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new ApiResponse<object> { Success = false };

        switch (exception)
        {
            case BusinessException e:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = e.Message;
                break;
            case NotFoundException e:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Message = e.Message;
                break;
            case ValidationException e:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "Errores de validación";
                response.Errors = e.Errors.Select(err => err.ErrorMessage);
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = "Ocurrió un error interno en el servidor.";
                break;
        }

        var result = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return context.Response.WriteAsync(result);
    }
}
