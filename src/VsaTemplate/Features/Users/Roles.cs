using System.Collections.Frozen;

namespace VsaTemplate.Features.Users;

public abstract class Roles
{
    public const string User = nameof(User);
    public const string Administrator = nameof(Administrator);

    public static FrozenSet<string> All = [User, Administrator];
}
