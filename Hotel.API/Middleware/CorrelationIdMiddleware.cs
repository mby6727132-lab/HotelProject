namespace Hotel.API.Middleware;

public class CorrelationIdMiddleware
{
    public const string CorrelationIdHeader = "X-Correlation-Id";
    public const string CorrelationIdItemKey = "CorrelationId";

    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = ResolveCorrelationId(context);
        context.Items[CorrelationIdItemKey] = correlationId;

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationIdHeader] = correlationId;
            return Task.CompletedTask;
        });

        using (_logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            _logger.LogInformation("Request started: {Method} {Path}", context.Request.Method, context.Request.Path);

            await _next(context);

            _logger.LogInformation(
                "Request completed: {Method} {Path} responded {StatusCode}",
                context.Request.Method, context.Request.Path, context.Response.StatusCode);
        }
    }

    // A caller-supplied id is honored only if it is a valid GUID; otherwise a new one is generated.
    private static string ResolveCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out var values) &&
            Guid.TryParse(values.ToString(), out var parsed))
        {
            return parsed.ToString();
        }

        return Guid.NewGuid().ToString();
    }
}
