using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Http;

/// <summary>
/// Common Class for Global Exception Handling
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var status = StatusCodes.Status500InternalServerError;

        if (exception is ValidationException)
        {
            status = StatusCodes.Status400BadRequest;
        }

        var response = new ProblemDetails
        {
            Status = status,
            Title = exception.Message,
            Type = exception.GetType().ToString(),
            Detail = exception.StackTrace
        };

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}