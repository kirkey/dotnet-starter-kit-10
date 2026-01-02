using FSH.Framework.Core.Domain;

namespace FSH.Modules.Todo.Domain;

/// <summary>
/// Master entity: Represents a Todo item with tasks
/// </summary>
public class Todo : AuditableEntity<Guid>
{
    // Todo-specific Properties
    public TodoPriority Priority { get; private set; } = TodoPriority.Medium;
    public DateTimeOffset? DueDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }

    // Navigation property - Detail relationship
    public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();

    private Todo() { } // EF Core

    /// <summary>
    /// Factory method to create a new Todo
    /// </summary>
    public static Todo Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        TodoPriority priority = TodoPriority.Medium,
        DateTimeOffset? dueDate = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);

        return new Todo
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Priority = priority,
            DueDate = dueDate,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            Status = TodoStatus.NotStarted.ToString(),
            IsActive = true,
            IsCompleted = false
        };
    }

    /// <summary>
    /// Update Todo details
    /// </summary>
    public void Update(
        string name,
        string? description,
        TodoPriority priority,
        DateTimeOffset? dueDate,
        Guid modifiedBy,
        string modifiedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }

    /// <summary>
    /// Mark Todo as completed
    /// </summary>
    public void Complete()
    {
        IsCompleted = true;
        CompletedAt = DateTimeOffset.UtcNow;
        Status = TodoStatus.Completed.ToString();
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Reopen a completed Todo
    /// </summary>
    public void Reopen()
    {
        IsCompleted = false;
        CompletedAt = null;
        Status = TodoStatus.InProgress.ToString();
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Update Todo status
    /// </summary>
    public void UpdateStatus(TodoStatus status)
    {
        Status = status.ToString();
        LastModifiedOnUtc = DateTimeOffset.UtcNow;

        if (status == TodoStatus.Completed && !IsCompleted)
        {
            Complete();
        }
    }
}
