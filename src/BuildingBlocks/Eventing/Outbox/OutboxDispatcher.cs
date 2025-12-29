using FSH.Framework.Eventing.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.Framework.Eventing.Outbox;

/// <summary>
/// Dispatches outbox messages via the configured event bus.
/// This type is intended to be invoked by a scheduler (e.g., Hangfire recurring job or hosted service).
/// </summary>
public sealed class OutboxDispatcher
{
    private readonly IOutboxStore _outbox;
    private readonly IEventBus _bus;
    private readonly IEventSerializer _serializer;
    private readonly ILogger<OutboxDispatcher> _logger;
    private readonly EventingOptions _options;

    public OutboxDispatcher(
        IOutboxStore outbox,
        IEventBus bus,
        IEventSerializer serializer,
        IOptions<EventingOptions> options,
        ILogger<OutboxDispatcher> logger)
    {
        ArgumentNullException.ThrowIfNull(options);

        _outbox = outbox;
        _bus = bus;
        _serializer = serializer;
        _logger = logger;
        _options = options.Value;
    }

    public async Task DispatchAsync(CancellationToken ct = default)
    {
        int batchSize = _options.OutboxBatchSize;
        if (batchSize <= 0) batchSize = 100;

        IReadOnlyList<OutboxMessage> messages = await _outbox.GetPendingBatchAsync(batchSize, ct).ConfigureAwait(false);
        if (messages.Count == 0)
        {
            _logger.LogDebug("No outbox messages to dispatch.");
            return;
        }

        _logger.LogInformation("Dispatching {Count} outbox messages (BatchSize={BatchSize})", messages.Count, batchSize);

        int processedCount = 0;
        int failedCount = 0;
        int deadLetterCount = 0;

        foreach (OutboxMessage message in messages)
        {
            try
            {
                IIntegrationEvent? @event = _serializer.Deserialize(message.Payload, message.Type);
                if (@event is null)
                {
                    await _outbox.MarkAsFailedAsync(message, "Cannot deserialize integration event.", isDead: true, ct).ConfigureAwait(false);
                    continue;
                }

                await _bus.PublishAsync(@event, ct).ConfigureAwait(false);
                await _outbox.MarkAsProcessedAsync(message, ct).ConfigureAwait(false);
                processedCount++;

                _logger.LogDebug("Outbox message {MessageId} dispatched and marked as processed.", message.Id);
            }
            catch (Exception ex)
            {
                int maxRetries = _options.OutboxMaxRetries <= 0 ? 5 : _options.OutboxMaxRetries;
                bool isDead = message.RetryCount + 1 >= maxRetries;

                await _outbox.MarkAsFailedAsync(message, ex.Message, isDead, ct).ConfigureAwait(false);

                failedCount++;
                if (isDead)
                {
                    deadLetterCount++;
                }

                if (isDead)
                {
                    _logger.LogError(ex, "Outbox message {MessageId} moved to dead-letter after {RetryCount} retries", message.Id, message.RetryCount + 1);
                }
                else
                {
                    _logger.LogWarning(ex, "Outbox message {MessageId} failed (RetryCount={RetryCount}).", message.Id, message.RetryCount + 1);
                }
            }
        }

        _logger.LogInformation(
            "Outbox dispatch summary: Total={Total}, Processed={Processed}, Failed={Failed}, DeadLettered={DeadLettered}",
            messages.Count,
            processedCount,
            failedCount,
            deadLetterCount);
    }
}
