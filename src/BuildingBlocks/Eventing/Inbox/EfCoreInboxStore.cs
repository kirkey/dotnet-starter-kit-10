using Microsoft.EntityFrameworkCore;

namespace FSH.Framework.Eventing.Inbox;

/// <summary>
/// EF Core-based inbox store for a specific DbContext.
/// </summary>
/// <typeparam name="TDbContext">The DbContext that owns the InboxMessages set.</typeparam>
public sealed class EfCoreInboxStore<TDbContext>(TDbContext dbContext) : IInboxStore
    where TDbContext : DbContext
{
    public async Task<bool> HasProcessedAsync(Guid eventId, string handlerName, CancellationToken ct = default)
    {
        return await dbContext.Set<InboxMessage>()
            .AnyAsync(i => i.Id == eventId && i.HandlerName == handlerName, ct)
            .ConfigureAwait(false);
    }

    public async Task MarkProcessedAsync(Guid eventId, string handlerName, string? tenantId, string eventType, CancellationToken ct = default)
    {
        InboxMessage message = new()
        {
            Id = eventId,
            EventType = eventType,
            HandlerName = handlerName,
            TenantId = tenantId,
            ProcessedOnUtc = DateTime.UtcNow
        };

        await dbContext.Set<InboxMessage>().AddAsync(message, ct).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}

