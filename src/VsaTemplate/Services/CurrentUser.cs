using System.Collections.Immutable;
using System.Security.Claims;
using VsaTemplate.Features.Users;

namespace VsaTemplate.Common.Models;

public sealed class CurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? Id
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User.FindFirstValue(  
                ClaimTypes.NameIdentifier
            );

            if (id is null)
                return null;

            if (Guid.TryParse(id, out var guid))
                return guid;

            //TODO: make this more specific exception
            throw new Exception(
                $"Failed to parse non-null {nameof(User)}.{nameof(User.Id)} to {nameof(Guid)}. Id: {id}"
            );
        }
    }

    public ImmutableArray<string>? Roles =>
        _httpContextAccessor
            .HttpContext?.User.FindAll(ClaimTypes.Role)
            .Select(x => x.Value)
            .ToImmutableArray();
}
