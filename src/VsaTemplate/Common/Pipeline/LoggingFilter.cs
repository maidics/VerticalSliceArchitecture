using VsaTemplate.Common.Interfaces;
using VsaTemplate.Common.Interfaces.Features;
using VsaTemplate.Common.Models;
using VsaTemplate.Infrastructure;

namespace VsaTemplate.Common.Pipeline;

public sealed class LoggingFilter : IEndpointFilter
{
    private readonly ILogger<LoggingFilter> _logger;
    private readonly IUser _user;

    public LoggingFilter(ILogger<LoggingFilter> logger, CurrentUser user)
    {
        _logger = logger;
        _user = user;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var request = context.Arguments.OfType<IRequest>().FirstOrDefault();

        var result = await next(context);

        _logger.LogInformation(
            "Request: {HttpMethod} {Path}, {@UserId}, {@Request}, {@ResponseStatusCode}",
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path.Value,
            _user.Id,
            request is null ? "none" : request,
            (result as IStatusCodeHttpResult)?.StatusCode
        );

        return result;
    }
}
