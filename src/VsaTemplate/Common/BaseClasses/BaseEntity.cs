using System.ComponentModel.DataAnnotations.Schema;
using VsaTemplate.Common.Interfaces.Features;

namespace VsaTemplate.Common.BaseClasses;

//credit: Jason Taylor
public abstract class BaseEntity
{
    // This can easily be modified to BaseEntity<T> to support different types for Id
    // Using string for simplicity
    public string Id { get; set; } = Guid.NewGuid().ToString();

    private readonly List<IDomainEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
