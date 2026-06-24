using System.ComponentModel.DataAnnotations.Schema;
using VsaTemplate.Common.Interfaces.Features;

namespace VsaTemplate.Common.BaseClasses;

//credit: https://github.com/jasontaylordev/CleanArchitecture
public abstract class BaseEntity
{
    // This can easily be modified to BaseEntity<T> to support different types for Id
    // Using Guid for type safety
    public Guid Id { get; set; } = Guid.NewGuid();

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
