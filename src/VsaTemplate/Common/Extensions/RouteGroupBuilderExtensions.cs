using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using VsaTemplate.Features.Examples;
using VsaTemplate.Features.Users;

namespace VsaTemplate.Common.Extensions;

/*
public static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder builder)
    {
        ExampleEndpoints.Map(builder);

        return builder.MapLogoutEndpoint();
    }

    private static RouteGroupBuilder MapLogoutEndpoint(this RouteGroupBuilder builder)
    {
        builder.MapPost("/identity/logout", Logout).WithTags("Users").RequireAuthorization();

        return builder;

        static async Task<Ok> Logout(SignInManager<ApplicationUser> signInManager)
        {
            await signInManager.SignOutAsync();
            return TypedResults.Ok();
        }
    }
}
*/
