using System.Collections.Frozen;

namespace VsaTemplate.Common.Interfaces;

public interface IUser
{
    string? Id { get; }
    FrozenSet<string>? Roles { get; }
}
