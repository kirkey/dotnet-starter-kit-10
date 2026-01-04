namespace FSH.Module.Todos.Events;

/// <summary>
/// Base class for all Todo domain events.
/// 
/// **Purpose:**
/// Provides a common base for domain events that track important state changes
/// in the Todo module. Can be extended to support event sourcing and event streaming.
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
/// Domain events are published when important todo state changes occur.
/// Handlers can subscribe to these events for side effects.
/// </summary>
public abstract class TodoDomainEvent
{
    /// <summary>
    /// Gets the timestamp when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets the ID of the aggregate (Todo) that triggered this event.
    /// </summary>
    public Guid AggregateId { get; protected set; }

    /// <summary>
    /// Gets the type of aggregate (for serialization/routing).
    /// </summary>
    public virtual string AggregateType => nameof(Domain.Todo);
}

/// <summary>
/// Event raised when a Todo has been created.
/// 
/// **Trigger:** After successful Todo creation
/// **Handlers:** Can send notifications, update search index, log activity
/// **Usage:** Subscribe to track all new todos created in the system
/// </summary>
public sealed class TodoCreatedEvent : TodoDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoCreatedEvent.
    /// </summary>
    /// <param name="todoId">The ID of the created todo.</param>
    /// <param name="name">The name of the created todo.</param>
    /// <param name="createdBy">The user ID who created the todo.</param>
    public TodoCreatedEvent(Guid todoId, string name, Guid createdBy)
    {
        AggregateId = todoId;
        Name = name;
        CreatedBy = createdBy;
    }

    /// <summary>
    /// Gets the name of the created todo.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the user ID who created the todo.
    /// </summary>
    public Guid CreatedBy { get; }
}

/// <summary>
/// Event raised when a Todo has been updated.
/// 
/// **Trigger:** After successful Todo update
/// **Handlers:** Can update search index, send notifications, log changes
/// **Usage:** Track modifications to todos
/// </summary>
public sealed class TodoUpdatedEvent : TodoDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoUpdatedEvent.
    /// </summary>
    /// <param name="todoId">The ID of the updated todo.</param>
    /// <param name="name">The updated name.</param>
    /// <param name="modifiedBy">The user ID who modified the todo.</param>
    public TodoUpdatedEvent(Guid todoId, string name, Guid modifiedBy)
    {
        AggregateId = todoId;
        Name = name;
        ModifiedBy = modifiedBy;
    }

    /// <summary>
    /// Gets the updated name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the user ID who modified the todo.
    /// </summary>
    public Guid ModifiedBy { get; }
}

/// <summary>
/// Event raised when a Todo has been marked as completed.
/// 
/// **Trigger:** After marking a todo as complete
/// **Handlers:** Can update metrics, send completion notifications, update dashboards
/// **Usage:** Track completed todos for analytics and user notifications
/// </summary>
public sealed class TodoCompletedEvent : TodoDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoCompletedEvent.
    /// </summary>
    /// <param name="todoId">The ID of the completed todo.</param>
    /// <param name="completedAt">The completion timestamp.</param>
    /// <param name="completedBy">The user ID who marked it complete.</param>
    public TodoCompletedEvent(Guid todoId, DateTimeOffset completedAt, Guid completedBy)
    {
        AggregateId = todoId;
        CompletedAt = completedAt;
        CompletedBy = completedBy;
    }

    /// <summary>
    /// Gets the completion timestamp.
    /// </summary>
    public DateTimeOffset CompletedAt { get; }

    /// <summary>
    /// Gets the user ID who marked the todo as complete.
    /// </summary>
    public Guid CompletedBy { get; }
}

/// <summary>
/// Event raised when a Todo has been reopened.
/// 
/// **Trigger:** After reopening a previously completed todo
/// **Handlers:** Can update metrics, send notifications
/// **Usage:** Track incomplete status transitions
/// </summary>
public sealed class TodoReopenedEvent : TodoDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoReopenedEvent.
    /// </summary>
    /// <param name="todoId">The ID of the reopened todo.</param>
    /// <param name="reopenedBy">The user ID who reopened the todo.</param>
    public TodoReopenedEvent(Guid todoId, Guid reopenedBy)
    {
        AggregateId = todoId;
        ReopenedBy = reopenedBy;
    }

    /// <summary>
    /// Gets the user ID who reopened the todo.
    /// </summary>
    public Guid ReopenedBy { get; }
}

/// <summary>
/// Event raised when a Todo has been deleted.
/// 
/// **Trigger:** After hard-deleting a todo
/// **Handlers:** Can clean up related data, update indexes, log deletions
/// **Usage:** Track todo deletions for compliance and auditing
/// </summary>
public sealed class TodoDeletedEvent : TodoDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoDeletedEvent.
    /// </summary>
    /// <param name="todoId">The ID of the deleted todo.</param>
    /// <param name="deletedBy">The user ID who deleted the todo.</param>
    public TodoDeletedEvent(Guid todoId, Guid deletedBy)
    {
        AggregateId = todoId;
        DeletedBy = deletedBy;
    }

    /// <summary>
    /// Gets the user ID who deleted the todo.
    /// </summary>
    public Guid DeletedBy { get; }
}

/// <summary>
/// Event raised when a Todo has been archived.
/// 
/// **Trigger:** After archiving a todo (soft delete)
/// **Handlers:** Can update visibility, send notifications
/// **Usage:** Track archived todos for recovery and compliance
/// </summary>
public sealed class TodoArchivedEvent : TodoDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoArchivedEvent.
    /// </summary>
    /// <param name="todoId">The ID of the archived todo.</param>
    /// <param name="archivedBy">The user ID who archived the todo.</param>
    public TodoArchivedEvent(Guid todoId, Guid archivedBy)
    {
        AggregateId = todoId;
        ArchivedBy = archivedBy;
    }

    /// <summary>
    /// Gets the user ID who archived the todo.
    /// </summary>
    public Guid ArchivedBy { get; }
}
