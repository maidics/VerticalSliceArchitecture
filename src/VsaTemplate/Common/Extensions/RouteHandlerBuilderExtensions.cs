using Microsoft.AspNetCore.Authorization;
using VsaTemplate.Common.Pipeline;

namespace VsaTemplate.Common.Extensions;

public static class RouteHandlerBuilderExtensions
{
    extension(RouteHandlerBuilder builder)
    {
        public RouteHandlerBuilder AddValidationFilter<TRequest>()
            where TRequest : notnull
        {
            return builder
                .AddEndpointFilter<ValidationFilter<TRequest>>()
                .ProducesValidationProblem();
        }

        public RouteHandlerBuilder RequireAuthorization(params string[] roles)
        {
            return builder.RequireAuthorization(
                new AuthorizeAttribute() { Roles = string.Join(",", roles) }
            );
        }
    }
}
