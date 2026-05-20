namespace VsaTemplate.Common.Interfaces;

public interface IMiddleware
{
    Task InvokeAsync(HttpContext httpContext);
}
