using System.Collections.Frozen;

namespace VsaTemplate.Common.Interfaces;

public interface IUser
{
    Guid? Id { get; }
    FrozenSet<string>? Roles { get; }
}
