using Microsoft.Extensions.Primitives;

namespace BGService.Api;

public class ApiKeyMiddleware
{
    private const string ApiKeyHeaderName = "X-API-Key";
    private const string ApiKeyConfigKey = "Api:Key";
    private readonly RequestDelegate _next;
    private static readonly HashSet<string> ExcludedPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/api/health"
    };

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (ExcludedPaths.Contains(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
        var validApiKey = configuration[ApiKeyConfigKey];

        if (string.IsNullOrEmpty(validApiKey))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out StringValues apiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "API Key is required" });
            return;
        }

        if (!string.Equals(apiKey, validApiKey, StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid API Key" });
            return;
        }

        await _next(context);
    }
}