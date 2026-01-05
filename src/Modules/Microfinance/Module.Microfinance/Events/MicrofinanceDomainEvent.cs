namespace FSH.Module.Microfinance.Events;

/// <summary>
/// Base class for all Microfinance domain events.
/// 
/// **Purpose:**
/// Provides a common base for domain events that track important state changes
/// in the Microfinance module. Can be extended to support event sourcing and event streaming.
/// 
/// **Design Pattern:**
/// Implements Domain Event pattern from DDD (Domain-Driven Design).
/// Events represent important business occurrences that have happened.
/// 
/// **Future Extensions:**
/// Can be integrated with:
/// - Event sourcing for audit trails
/// - Event streaming (Kafka, RabbitMQ)
/// - Event-driven microservices
/// - CQRS event store
/// 
/// **Usage:**
/// Domain events are published when important entity state changes occur.
/// Handlers can subscribe to these events for side effects.
/// </summary>
public abstract class MicrofinanceDomainEvent
{
    /// <summary>
    /// Gets the timestamp when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets the ID of the aggregate that triggered this event.
    /// </summary>
    public Guid AggregateId { get; protected set; }

    /// <summary>
    /// Gets the type of aggregate (for serialization/routing).
    /// </summary>
    public abstract string AggregateType { get; }
}
