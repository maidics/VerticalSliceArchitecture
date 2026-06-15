using System.Collections.Frozen;
using System.Security.Claims;
using VsaTemplate.Common.Interfaces;

namespace VsaTemplate.Infrastructure;

public sealed class CurrentUser : IUser
{
    public string? Id { get; }
    public FrozenSet<string>? Roles { get; }

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return;
        }

        Id = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        Roles = httpContextAccessor
            .HttpContext.User.FindAll(ClaimTypes.Role)
            .Select(x => x.Value)
            .ToFrozenSet();
    }
}
