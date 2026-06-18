using Microsoft.EntityFrameworkCore.Diagnostics;
using VsaTemplate.Common.BaseClasses;
using VsaTemplate.Infrastructure;

namespace VsaTemplate.Database.Interceptors;

//credit: Jason Taylor
public sealed class DispatchDomainEventInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher _dispatcher;

    public DispatchDomainEventInterceptor(IDomainEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        DispatchDomainEventsAsync(eventData.Context).GetAwaiter().GetResult();

        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await DispatchDomainEventsAsync(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public async Task DispatchDomainEventsAsync(DbContext? context)
    {
        if (context is null)
            return;

        var entities = context
            .ChangeTracker.Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity);

        var events = entities.SelectMany(x => x.DomainEvents).ToList();

        entities.ToList().ForEach(x => x.ClearDomainEvents());

        foreach (var @event in events)
        {
            await _dispatcher.DispatchAsync(@event, CancellationToken.None);
        }
    }
}
