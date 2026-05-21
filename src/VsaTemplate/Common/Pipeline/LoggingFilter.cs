using VsaTemplate.Common.Interfaces;
using VsaTemplate.Common.Models;

namespace VsaTemplate.Common.Pipeline;

public sealed class LoggingFilter : IEndpointFilter
{
    private readonly ILogger<LoggingFilter> _logger;
    private readonly CurrentUser _user;

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
            "Request: {Path} [{HttpMethod}], {@UserId}, {@Request}, {@Result}",
            context.HttpContext.Request.Path.Value,
            context.HttpContext.Request.Method,
            _user.Id,
            request is null ? "none" : request,
            GetLoggableResult(result)
        );

        return result;
    }

    //TODO: this returns null for status code & "no content" for result value always
    private static object GetLoggableResult(object? result)
    {
        if (result is null)
            return "no result";

        var statusCode = (result as IStatusCodeHttpResult)?.StatusCode;
        var resultValue = (result as IValueHttpResult)?.Value ?? "no content";

        return new { StatusCode = statusCode, Body = resultValue };
    }
}
