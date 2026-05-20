using System.Diagnostics;
using VsaTemplate.Common.Models;
using IMiddleware = VsaTemplate.Common.Interfaces.IMiddleware;

namespace VsaTemplate.Common.Pipeline;

public sealed class PerformanceMiddleware : IMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Stopwatch _timer;
    private readonly CurrentUser _user;
    private readonly ILogger<LoggingMiddleware> _logger;

    public PerformanceMiddleware(
        RequestDelegate next,
        CurrentUser user,
        ILogger<LoggingMiddleware> logger
    )
    {
        _next = next;
        _timer = new Stopwatch();
        _user = user;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        _timer.Start();

        await _next(httpContext);

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            _logger.LogWarning(
                "VsaTemplate running request: {Path}, ({ElapsedMilliseconds}ms), {UserId}",
                httpContext.Request.Path,
                elapsedMilliseconds,
                _user.Id
            );
        }
    }
}
