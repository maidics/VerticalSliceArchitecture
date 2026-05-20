using VsaTemplate.Common.Models;
using IMiddleware = VsaTemplate.Common.Interfaces.IMiddleware;

namespace VsaTemplate.Common.Pipeline;

public class LoggingMiddleware : IMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CurrentUser _user;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(
        RequestDelegate next,
        CurrentUser user,
        ILogger<LoggingMiddleware> logger
    )
    {
        _next = next;
        _user = user;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        _logger.LogInformation(
            "VsaTemplate request: {Path}, {UserId}",
            httpContext.Request.Path,
            _user.Id
        );

        await _next(httpContext);
    }
}
