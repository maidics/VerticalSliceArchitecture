using System.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using VsaTemplate.Common.Interfaces;
using VsaTemplate.Features.Users;

namespace VsaTemplate.Common.Extensions;

// credit: Jason Taylor
public static class WebApplicationExtensions
{
    extension(WebApplication app)
    {
        public WebApplication MapEndpoints(Assembly assembly)
        {
            var endpointGroupTypes = assembly
                .GetExportedTypes()
                .Where(t =>
                    t is { IsAbstract: false, IsInterface: false }
                    && t.IsAssignableTo(typeof(IEndpointGroup))
                );

            foreach (var type in endpointGroupTypes)
            {
                var groupName = type.Name;
                var routePrefix =
                    type.GetProperty(nameof(IEndpointGroup.RoutePrefix))?.GetValue(null) as string
                    ?? $"/api/{groupName}";
                var group = app.MapGroup(routePrefix).WithTags(groupName);
                type.GetMethod(nameof(IEndpointGroup.Map))!.Invoke(null, [group]);
            }

            return app.MapLogoutEndpoint();
        }

        private WebApplication MapLogoutEndpoint()
        {
            app.MapPost("api/identity/logout", Logout).WithTags("Users").RequireAuthorization();

            return app;

            static async Task<Ok> Logout(SignInManager<User> signInManager)
            {
                await signInManager.SignOutAsync();
                return TypedResults.Ok();
            }
        }
    }
}
