using FSH.Framework.Core.Domain;

namespace FSH.Modules.Todo.Domain;

/// <summary>
/// Detail entity: Represents a task under a Todo
/// </summary>
public class TodoTask : AuditableEntity<Guid>
{
    // TodoTask-specific Properties
    public Guid TodoId { get; private set; } // Foreign Key
    public bool IsCompleted { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public int SortOrder { get; private set; }

    // Navigation property
    public virtual Todo Todo { get; set; } = default!;

    private TodoTask() { } // EF Core

    /// <summary>
    /// Factory method to create a new TodoTask
    /// </summary>
    public static TodoTask Create(
        Guid todoId,
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        int sortOrder = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);

        return new TodoTask
        {
            Id = Guid.NewGuid(),
            TodoId = todoId,
            Name = name,
            Description = description,
            SortOrder = sortOrder,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            Status = "Pending",
            IsActive = true,
            IsCompleted = false
        };
    }

    /// <summary>
    /// Update TodoTask details
    /// </summary>
    public void Update(
        string name,
        string? description,
        int sortOrder,
        Guid modifiedBy,
        string modifiedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Description = description;
        SortOrder = sortOrder;
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }

    /// <summary>
    /// Mark task as completed
    /// </summary>
    public void Complete()
    {
        IsCompleted = true;
        CompletedAt = DateTimeOffset.UtcNow;
        Status = "Completed";
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Reopen a completed task
    /// </summary>
    public void Reopen()
    {
        IsCompleted = false;
        CompletedAt = null;
        Status = "Pending";
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Toggle task completion status
    /// </summary>
    public void ToggleCompletion()
    {
        if (IsCompleted)
        {
            Reopen();
        }
        else
        {
            Complete();
        }
    }
}
