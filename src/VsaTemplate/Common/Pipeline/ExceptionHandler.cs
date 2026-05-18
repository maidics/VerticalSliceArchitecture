using Microsoft.AspNetCore.Diagnostics;

namespace VsaTemplate.Common.Pipeline;

public class ExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
