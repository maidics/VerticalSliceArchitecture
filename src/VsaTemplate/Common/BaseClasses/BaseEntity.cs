namespace VsaTemplate.Common.BaseClasses;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
