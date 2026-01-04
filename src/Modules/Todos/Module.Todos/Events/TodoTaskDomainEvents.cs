namespace FSH.Module.Todos.Events;

/// <summary>
/// Base class for all TodoTask domain events.
/// 
/// **Purpose:**
/// Provides a common base for domain events related to TodoTask entities.
/// Enables tracking of task-level state changes within todos.
/// 
/// **Design Pattern:**
/// Implements Domain Event pattern from DDD.
/// Events represent important business occurrences for todo tasks.
/// 
/// **Future Extensions:**
/// Can be integrated with event sourcing and event streaming for tasks.
/// </summary>
public abstract class TodoTaskDomainEvent
{
    /// <summary>
    /// Gets the timestamp when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets the ID of the task that triggered this event.
    /// </summary>
    public Guid TaskId { get; protected set; }

    /// <summary>
    /// Gets the ID of the parent Todo.
    /// </summary>
    public Guid TodoId { get; protected set; }

    /// <summary>
    /// Gets the type of aggregate (for serialization/routing).
    /// </summary>
    public virtual string AggregateType => nameof(Domain.TodoTask);
}

/// <summary>
/// Event raised when a TodoTask has been created.
/// 
/// **Trigger:** After successful task creation
/// **Handlers:** Can update todo metrics, send notifications
/// **Usage:** Track new tasks added to todos
/// </summary>
public sealed class TodoTaskCreatedEvent : TodoTaskDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoTaskCreatedEvent.
    /// </summary>
    /// <param name="taskId">The ID of the created task.</param>
    /// <param name="todoId">The ID of the parent todo.</param>
    /// <param name="name">The name of the created task.</param>
    /// <param name="createdBy">The user ID who created the task.</param>
    public TodoTaskCreatedEvent(Guid taskId, Guid todoId, string name, Guid createdBy)
    {
        TaskId = taskId;
        TodoId = todoId;
        Name = name;
        CreatedBy = createdBy;
    }

    /// <summary>
    /// Gets the name of the created task.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the user ID who created the task.
    /// </summary>
    public Guid CreatedBy { get; }
}

/// <summary>
/// Event raised when a TodoTask has been updated.
/// 
/// **Trigger:** After successful task update
/// **Handlers:** Can update search index, log changes
/// **Usage:** Track modifications to tasks
/// </summary>
public sealed class TodoTaskUpdatedEvent : TodoTaskDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoTaskUpdatedEvent.
    /// </summary>
    /// <param name="taskId">The ID of the updated task.</param>
    /// <param name="todoId">The ID of the parent todo.</param>
    /// <param name="name">The updated name.</param>
    /// <param name="modifiedBy">The user ID who modified the task.</param>
    public TodoTaskUpdatedEvent(Guid taskId, Guid todoId, string name, Guid modifiedBy)
    {
        TaskId = taskId;
        TodoId = todoId;
        Name = name;
        ModifiedBy = modifiedBy;
    }

    /// <summary>
    /// Gets the updated name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the user ID who modified the task.
    /// </summary>
    public Guid ModifiedBy { get; }
}

/// <summary>
/// Event raised when a TodoTask has been marked as completed.
/// 
/// **Trigger:** After marking a task as complete
/// **Handlers:** Can update todo progress metrics, send notifications
/// **Usage:** Track completed tasks for progress tracking
/// </summary>
public sealed class TodoTaskCompletedEvent : TodoTaskDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoTaskCompletedEvent.
    /// </summary>
    /// <param name="taskId">The ID of the completed task.</param>
    /// <param name="todoId">The ID of the parent todo.</param>
    /// <param name="completedAt">The completion timestamp.</param>
    /// <param name="completedBy">The user ID who completed the task.</param>
    public TodoTaskCompletedEvent(Guid taskId, Guid todoId, DateTimeOffset completedAt, Guid completedBy)
    {
        TaskId = taskId;
        TodoId = todoId;
        CompletedAt = completedAt;
        CompletedBy = completedBy;
    }

    /// <summary>
    /// Gets the completion timestamp.
    /// </summary>
    public DateTimeOffset CompletedAt { get; }

    /// <summary>
    /// Gets the user ID who completed the task.
    /// </summary>
    public Guid CompletedBy { get; }
}

/// <summary>
/// Event raised when a TodoTask has been reopened.
/// 
/// **Trigger:** After reopening a completed task
/// **Handlers:** Can update metrics, send notifications
/// **Usage:** Track incomplete status transitions for tasks
/// </summary>
public sealed class TodoTaskReopenedEvent : TodoTaskDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoTaskReopenedEvent.
    /// </summary>
    /// <param name="taskId">The ID of the reopened task.</param>
    /// <param name="todoId">The ID of the parent todo.</param>
    /// <param name="reopenedBy">The user ID who reopened the task.</param>
    public TodoTaskReopenedEvent(Guid taskId, Guid todoId, Guid reopenedBy)
    {
        TaskId = taskId;
        TodoId = todoId;
        ReopenedBy = reopenedBy;
    }

    /// <summary>
    /// Gets the user ID who reopened the task.
    /// </summary>
    public Guid ReopenedBy { get; }
}

/// <summary>
/// Event raised when a TodoTask has been deleted.
/// 
/// **Trigger:** After deleting a task
/// **Handlers:** Can update metrics, log deletion
/// **Usage:** Track deleted tasks
/// </summary>
public sealed class TodoTaskDeletedEvent : TodoTaskDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoTaskDeletedEvent.
    /// </summary>
    /// <param name="taskId">The ID of the deleted task.</param>
    /// <param name="todoId">The ID of the parent todo.</param>
    /// <param name="deletedBy">The user ID who deleted the task.</param>
    public TodoTaskDeletedEvent(Guid taskId, Guid todoId, Guid deletedBy)
    {
        TaskId = taskId;
        TodoId = todoId;
        DeletedBy = deletedBy;
    }

    /// <summary>
    /// Gets the user ID who deleted the task.
    /// </summary>
    public Guid DeletedBy { get; }
}

/// <summary>
/// Event raised when TodoTasks have been reordered.
/// 
/// **Trigger:** After reordering tasks within a todo
/// **Handlers:** Can update search index, send notifications
/// **Usage:** Track task ordering changes
/// </summary>
public sealed class TodoTasksReorderedEvent : TodoTaskDomainEvent
{
    /// <summary>
    /// Initializes a new instance of TodoTasksReorderedEvent.
    /// </summary>
    /// <param name="todoId">The ID of the parent todo.</param>
    /// <param name="taskOrders">Dictionary of task IDs to new sort orders.</param>
    /// <param name="reorderedBy">The user ID who reordered the tasks.</param>
    public TodoTasksReorderedEvent(Guid todoId, Dictionary<Guid, int> taskOrders, Guid reorderedBy)
    {
        TodoId = todoId;
        TaskOrders = taskOrders;
        ReorderedBy = reorderedBy;
    }

    /// <summary>
    /// Gets the dictionary of task IDs to their new sort orders.
    /// </summary>
    public Dictionary<Guid, int> TaskOrders { get; }

    /// <summary>
    /// Gets the user ID who reordered the tasks.
    /// </summary>
    public Guid ReorderedBy { get; }
}
