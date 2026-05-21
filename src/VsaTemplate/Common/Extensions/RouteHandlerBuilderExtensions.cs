using Microsoft.AspNetCore.Authorization;

namespace VsaTemplate.Common.Extensions;

public static class RouteHandlerBuilderExtensions
{
    extension(RouteHandlerBuilder builder)
    {
        public RouteHandlerBuilder RequireAuthorizationWithRole(params string[] roles)
        {
            return builder.RequireAuthorization(
                new AuthorizeAttribute() { Roles = string.Join(",", roles) }
            );
        }
    }
}
