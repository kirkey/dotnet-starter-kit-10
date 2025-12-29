using FSH.Framework.Core.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace FSH.Framework.Persistence.Inteceptors;

public sealed class DomainEventsInterceptor(IPublisher publisher, ILogger<DomainEventsInterceptor> logger)
    : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventData);
        DbContext? context = eventData.Context;
        if (context == null)
            return await base.SavedChangesAsync(eventData, result, cancellationToken);

        IDomainEvent[] domainEvents = context.ChangeTracker
            .Entries<IHasDomainEvents>()
            .SelectMany(e =>
            {
                IDomainEvent[] pending = e.Entity.DomainEvents.ToArray();
                e.Entity.ClearDomainEvents();
                return pending;
            })
            .ToArray();

        if (domainEvents.Length == 0)
            return await base.SavedChangesAsync(eventData, result, cancellationToken);

        logger.LogDebug("Publishing {Count} domain events...", domainEvents.Length);

        foreach (IDomainEvent domainEvent in domainEvents)
            await publisher.Publish(domainEvent, cancellationToken).ConfigureAwait(false);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}