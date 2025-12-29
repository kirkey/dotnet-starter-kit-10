using FSH.Framework.Eventing.Abstractions;
using FSH.Framework.Eventing.Inbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace FSH.Framework.Eventing.InMemory;

/// <summary>
/// In-memory event bus implementation used for single-process deployments.
/// It resolves handlers from DI and optionally uses an inbox store for idempotency.
/// </summary>
public sealed class InMemoryEventBus(IServiceProvider serviceProvider, ILogger<InMemoryEventBus> logger)
    : IEventBus
{
    public Task PublishAsync(IIntegrationEvent @event, CancellationToken ct = default)
        => PublishAsync(new[] { @event }, ct);

    public async Task PublishAsync(IEnumerable<IIntegrationEvent> events, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(events);

        foreach (IIntegrationEvent @event in events)
        {
            await PublishSingleAsync(@event, ct).ConfigureAwait(false);
        }
    }

    private async Task PublishSingleAsync(IIntegrationEvent @event, CancellationToken ct)
    {
        Type eventType = @event.GetType();
        logger.LogDebug("Publishing integration event {EventType} ({EventId})", eventType.FullName, @event.Id);

        using IServiceScope scope = serviceProvider.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;

        Type handlerInterfaceType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
        object?[] handlers = provider.GetServices(handlerInterfaceType).ToArray();

        if (handlers.Length == 0)
        {
            logger.LogDebug("No handlers registered for integration event type {EventType}", eventType.FullName);
            return;
        }

        IInboxStore? inbox = provider.GetService<IInboxStore>();

        foreach (object? handler in handlers)
        {
            if (handler is null)
            {
                continue;
            }

            string handlerName = handler.GetType().FullName ?? handler.GetType().Name;

            if (inbox != null)
            {
                if (await inbox.HasProcessedAsync(@event.Id, handlerName, ct).ConfigureAwait(false))
                {
                    logger.LogDebug("Skipping already processed integration event {EventId} for handler {Handler}", @event.Id, handlerName);
                    continue;
                }
            }

            MethodInfo? method = handlerInterfaceType.GetMethod(nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync));
            if (method == null)
            {
                logger.LogWarning("Handler {Handler} does not implement HandleAsync correctly for {EventType}", handlerName, eventType.FullName);
                continue;
            }

            try
            {
                Task task = (Task)method.Invoke(handler, new object[] { @event, ct })!;
                await task.ConfigureAwait(false);

                if (inbox != null)
                {
                    await inbox.MarkProcessedAsync(@event.Id, handlerName, @event.TenantId, eventType.AssemblyQualifiedName ?? eventType.FullName!, ct)
                        .ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while handling integration event {EventId} with handler {Handler}", @event.Id, handlerName);
                throw;
            }
        }
    }
}
