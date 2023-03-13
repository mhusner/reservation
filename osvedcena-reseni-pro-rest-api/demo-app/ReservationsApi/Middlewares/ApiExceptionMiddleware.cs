using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReservationsApi.Exceptions;
using ReservationsApi.Services;

namespace ReservationsApi.Middlewares;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _host;

    public ApiExceptionMiddleware(RequestDelegate next, IHostEnvironment host)
    {
        _next = next;
        _host = host;
    }

    public async Task InvokeAsync(HttpContext context, ApiValidationContext validationContext)
    {
        var id = Activity.Current?.Id ?? context.TraceIdentifier;
        context.Response.Headers.Add("RequestId", id);

        try
        {
            await _next(context);
        }
        catch (ApiValidationException e)
        {
            ProblemDetails problem = new();
            problem.Title = "Validation failed";
            problem.Status = StatusCodes.Status400BadRequest;
            problem.Extensions.Add("errors", validationContext.GetFailures().Select(x => new
            {
                x.PropertyName, x.ErrorMessage, x.AttemptedValue, x.ErrorCode
            }));

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (ApiNotFoundException e)
        {
            ProblemDetails problem = new();
            problem.Status = StatusCodes.Status404NotFound;
            problem.Title = "Object not found";

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (Exception e)
        {
            ProblemDetails problem = new();
            problem.Status = StatusCodes.Status500InternalServerError;
            problem.Title = "Internal server error";
            problem.Detail = _host.IsDevelopment() ? e.ToStringDemystified() : string.Empty;

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}