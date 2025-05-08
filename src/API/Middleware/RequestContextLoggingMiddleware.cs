using Serilog.Context;
using SharedKernel.Abstractions.Tracing;

namespace API.Middleware;

/// <summary>
/// Middleware for logging request context, including the correlation ID, to enhance traceability.
/// </summary>
/// <param name="next">The next middleware in the pipeline.</param>
public class RequestContextLoggingMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    /// <summary>
    /// Invokes the middleware to process the HTTP request and response, ensuring the correlation ID
    /// is included in the headers and logging context.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <param name="correlationIdAccessor">The accessor for managing the correlation ID.</param>
    public async Task InvokeAsync(HttpContext context, ICorrelationIdAccessor correlationIdAccessor)
    {
        var correlationId = GetCorrelationId(context, correlationIdAccessor);

        // Add the correlation ID to the request headers if not already present.
        context.Request.Headers.TryAdd(CorrelationIdHeaderName, correlationId);

        // Add the correlation ID to the response headers before the response is sent.
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.TryAdd(CorrelationIdHeaderName, correlationId);
            return Task.CompletedTask;
        });

        // Push the correlation ID into the logging context for the duration of the request.
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await next(context);
        }
    }

    /// <summary>
    /// Retrieves the correlation ID from the request headers or generates a new one if not present.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <param name="correlationIdAccessor">The accessor for managing the correlation ID.</param>
    /// <returns>The correlation ID as a string.</returns>
    private static string GetCorrelationId(HttpContext context, ICorrelationIdAccessor correlationIdAccessor)
    {
        if (context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId))
        {
            correlationIdAccessor.Set(correlationId!);
            return correlationId!;
        }

        return correlationIdAccessor.Get().ToString();
    }
}