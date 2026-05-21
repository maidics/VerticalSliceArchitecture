using System.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using VsaTemplate.Common.Interfaces;
using VsaTemplate.Features.Users;

namespace VsaTemplate.Common.Extensions;

public static class IEndpointRouteBuilderExtensions
{
    extension(IEndpointRouteBuilder builder)
    {
        public IEndpointRouteBuilder MapEndpoints()
        {
            //credit: Jason Taylor
            var endpointGroupTypes = Assembly
                .GetExecutingAssembly()
                .GetExportedTypes()
                .Where(t =>
                    t is { IsAbstract: false, IsInterface: false }
                    && t.IsAssignableTo(typeof(IEndpointGroup))
                );

            foreach (var type in endpointGroupTypes)
            {
                var groupName =
                    type.GetProperty(nameof(IEndpointGroup.RoutePrefix))?.GetValue(null) as string
                    ?? type.Name;
                var group = builder.MapGroup($"/{groupName}").WithTags(groupName);
                type.GetMethod(nameof(IEndpointGroup.Map))!.Invoke(null, [group]);
            }

            return builder.MapLogoutEndpoint();
        }

        private IEndpointRouteBuilder MapLogoutEndpoint()
        {
            builder.MapPost("/identity/logout", Logout).WithTags("Users").RequireAuthorization();

            return builder;

            static async Task<Ok> Logout(SignInManager<User> signInManager)
            {
                await signInManager.SignOutAsync();
                return TypedResults.Ok();
            }
        }
    }
}
