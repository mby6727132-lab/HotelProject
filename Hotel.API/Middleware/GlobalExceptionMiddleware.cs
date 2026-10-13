using System.Net;
using System.Text.Json;
using Hotel.Core.Exceptions;

namespace Hotel.API.Middleware;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, isHandled) = MapException(exception);
        var correlationId = context.Items[CorrelationIdMiddleware.CorrelationIdItemKey] as string;

        if (isHandled)
        {
            // A known business-rule failure that was turned into a clean 4xx response.
            _logger.LogWarning(
                "Request {Method} {Path} resulted in a handled {StatusCode} response: {Message}. CorrelationId={CorrelationId}",
                context.Request.Method, context.Request.Path, statusCode, message, correlationId);
        }
        else
        {
            // An unexpected failure - the client only ever sees a generic message, never ex details.
            _logger.LogError(
                exception,
                "Unhandled exception while processing {Method} {Path}. CorrelationId={CorrelationId}",
                context.Request.Method, context.Request.Path, correlationId);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var payload = new
        {
            status = statusCode,
            message,
            correlationId
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }

    private static (int StatusCode, string Message, bool IsHandled) MapException(Exception exception) => exception switch
    {
        InvalidDateRangeException => ((int)HttpStatusCode.BadRequest, exception.Message, true),
        InvalidCredentialsException => ((int)HttpStatusCode.Unauthorized, exception.Message, true),
        EmailAlreadyExistsException => ((int)HttpStatusCode.Conflict, exception.Message, true),
        RoomUnavailableException => ((int)HttpStatusCode.Conflict, exception.Message, true),
        ConcurrencyConflictException => ((int)HttpStatusCode.Conflict, exception.Message, true),
        KeyNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message, true),
        _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.", false)
    };
}
