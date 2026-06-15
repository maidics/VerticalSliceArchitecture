namespace VsaTemplate.Common.BaseClasses;

public abstract class BaseEntity
{
    // This can easily be modified to BaseEntity<T> to support different types for Id
    // Using string for simplicity
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
