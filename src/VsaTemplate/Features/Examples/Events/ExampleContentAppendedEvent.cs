using VsaTemplate.Common.Interfaces.Features;
using VsaTemplate.Database;

namespace VsaTemplate.Features.Examples.Events;

public sealed record ExampleContentAppendedEvent(Example Example) : IDomainEvent;

public sealed class ExampleContentAppendedEventHandler
    : IDomainEventHandler<ExampleContentAppendedEvent>
{
    private readonly ApplicationDbContext _context;

    public ExampleContentAppendedEventHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        ExampleContentAppendedEvent @event,
        CancellationToken cancellationToken
    )
    {
        @event.Example.HasAppendedContent = true;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
