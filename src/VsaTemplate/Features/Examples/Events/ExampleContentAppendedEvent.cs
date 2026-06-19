using VsaTemplate.Common.Interfaces.Features;
using VsaTemplate.Infrastructure.Database;

namespace VsaTemplate.Features.Examples.Events;

public sealed record ExampleContentAppendedEvent(string ExampleId) : IDomainEvent;

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
        var example = await _context.Examples.FirstOrDefaultAsync(
            x => x.Id == @event.ExampleId,
            cancellationToken
        );

        if (example is null)
            throw new InvalidOperationException($"Example not found: {@event.ExampleId}");

        example.HasAppendedContent = true;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
