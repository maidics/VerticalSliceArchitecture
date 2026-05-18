namespace VsaTemplate.Common.Pipeline;

public interface IMiddleware
{
    Task InvokeAsync(HttpContext httpContext);
}
