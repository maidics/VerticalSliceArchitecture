using System.ComponentModel.DataAnnotations.Schema;

namespace VsaTemplate.Common.BaseClasses;

// credit to: Jason Taylor
public abstract class BaseEntity
{
    public required Guid Id { get; set; } = Guid.NewGuid();

    private readonly List<BaseEvent> _events = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> Events => _events.AsReadOnly();

    public void AddEvent(BaseEvent @event)
    {
        _events.Add(@event);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }
}
