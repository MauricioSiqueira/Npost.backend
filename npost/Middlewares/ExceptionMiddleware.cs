using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


namespace npost.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            await HandleBusinessExceptionAsync(context, ex);
        }
        catch (Exception ex ) when (!_env.IsDevelopment())
        {
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private Task HandleBusinessExceptionAsync(HttpContext context, BusinessException exception)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var validationProblem = new ValidationProblemDetails()
        {
            Type = "Npost rules",
            Title = "Regra de negócio violada",
            Status = StatusCodes.Status400BadRequest
        };

        validationProblem.Errors.Add("business_error", new[] { exception.Message });

        var json = JsonSerializer.Serialize(validationProblem);
        return context.Response.WriteAsync(json);
    }

    private Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var problem = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Title = "An unexpected error occurred!",
            Detail = "",
            Status = StatusCodes.Status500InternalServerError
        };

        var json = JsonSerializer.Serialize(problem);
        return context.Response.WriteAsync(json);
    }
}