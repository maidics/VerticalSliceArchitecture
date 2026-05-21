using System.Diagnostics;
using VsaTemplate.Common.Models;

namespace VsaTemplate.Common.Pipeline;

public sealed class PerformanceFilter : IEndpointFilter
{
    private readonly ILogger<PerformanceFilter> _logger;
    private readonly CurrentUser _user;

    public PerformanceFilter(ILogger<PerformanceFilter> logger, CurrentUser user)
    {
        _logger = logger;
        _user = user;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var timer = Stopwatch.StartNew();

        var result = await next(context);

        timer.Stop();

        var elapsedMilliseconds = timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            _logger.LogWarning(
                "Long running request: {Path} [{HttpMethod}], {@UserId}, ({ElapsedMilliseconds}ms)",
                context.HttpContext.Request.Path,
                context.HttpContext.Request.Method,
                elapsedMilliseconds,
                _user.Id
            );
        }

        return result;
    }
}
