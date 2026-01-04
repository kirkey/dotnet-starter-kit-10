using FSH.Framework.Core.Domain;

namespace FSH.Module.Todos.Domain;

/// <summary>
/// Detail entity representing a task under a Todo item.
/// 
/// **Purpose:**
/// Represents a sub-task or work item within a Todo. TodoTasks allow breaking down larger todos
/// into smaller, manageable pieces of work with individual completion tracking and ordering.
/// 
/// **Business Logic:**
/// - Each TodoTask belongs to exactly one Todo (foreign key relationship)
/// - Tasks can be marked as completed independently of the parent Todo
/// - Tasks maintain a sort order for sequencing and prioritization
/// - Completion tracking includes both a boolean flag and a timestamp
/// - Tasks inherit audit tracking from the parent Todo creation context
/// 
/// **Relationships:**
/// - Detail in a one-to-many relationship with Todo entity
/// - Always has a parent Todo (required foreign key)
/// - Inherits audit tracking from parent Todo
/// 
/// **Multi-Tenancy:**
/// - Supports multi-tenancy through TenantId property (inherited from parent Todo context)
/// 
/// **Audit Trail:**
/// - Tracks creation details (CreatedBy, CreatedByUserName, CreatedOnUtc)
/// - Tracks modification details (LastModifiedBy, LastModifiedByUserName, LastModifiedOnUtc)
/// </summary>
public class TodoTask : AuditableEntity<Guid>
{
    /// <summary>
    /// Gets the ID of the parent Todo to which this task belongs (foreign key).
    /// </summary>
    public Guid TodoId { get; private set; } // Foreign Key
    
    /// <summary>
    /// Gets a value indicating whether this task has been completed.
    /// </summary>
    public bool IsCompleted { get; private set; }
    
    /// <summary>
    /// Gets the timestamp when this task was completed (nullable).
    /// Only set when IsCompleted is true.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; private set; }
    
    /// <summary>
    /// Gets the sort order of this task within its parent Todo.
    /// Used to determine the sequence of tasks in a list.
    /// Lower values appear first.
    /// </summary>
    public int SortOrder { get; private set; }

    /// <summary>
    /// Gets the parent Todo navigation property.
    /// Provides access to the Todo this task belongs to.
    /// </summary>
    public virtual Todo Todo { get; set; } = default!;

    private TodoTask() { } // EF Core

    /// <summary>
    /// Factory method to create a new TodoTask.
    /// 
    /// Initializes a new task with the provided details and sets default values for:
    /// - Status: Pending
    /// - IsActive: true
    /// - IsCompleted: false
    /// - SortOrder: 0 (if not specified)
    /// </summary>
    /// <param name="todoId">The ID of the parent Todo (required).</param>
    /// <param name="name">The name/title of the task (required, cannot be null or whitespace).</param>
    /// <param name="tenantId">The tenant ID for multi-tenancy support (required).</param>
    /// <param name="createdBy">The user ID of the person creating the task.</param>
    /// <param name="createdByUserName">The username of the person creating the task.</param>
    /// <param name="description">Optional description providing more details about the task.</param>
    /// <param name="sortOrder">The sort order position of this task within the parent Todo (defaults to 0).</param>
    /// <returns>A new TodoTask instance with all properties initialized.</returns>
    /// <exception cref="ArgumentException">Thrown when name or tenantId is null or whitespace.</exception>
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
    /// Updates the details of this task.
    /// 
    /// Updates the name, description, and sort order of the task.
    /// Automatically records the modification details.
    /// </summary>
    /// <param name="name">The new name/title (required, cannot be null or whitespace).</param>
    /// <param name="description">The new description (optional).</param>
    /// <param name="sortOrder">The new sort order position within the parent Todo.</param>
    /// <param name="modifiedBy">The user ID of the person making the modification.</param>
    /// <param name="modifiedByUserName">The username of the person making the modification.</param>
    /// <exception cref="ArgumentException">Thrown when name is null or whitespace.</exception>
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
    /// Marks this task as completed.
    /// 
    /// Sets IsCompleted to true, records the completion timestamp, and updates the status to Completed.
    /// This is typically called when the task work is finished.
    /// </summary>
    public void Complete()
    {
        IsCompleted = true;
        CompletedAt = DateTimeOffset.UtcNow;
        Status = "Completed";
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Reopens a previously completed task.
    /// 
    /// Sets IsCompleted to false, clears the completion timestamp, and updates the status to Pending.
    /// Use this when a completed task needs to be worked on again.
    /// </summary>
    public void Reopen()
    {
        IsCompleted = false;
        CompletedAt = null;
        Status = "Pending";
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Updates the sort order of the task.
    /// 
    /// Updates the position of the task within the parent Todo's task list.
    /// Automatically records modification details for audit trail.
    /// </summary>
    /// <param name="newSortOrder">The new sort order position.</param>
    public void UpdateSortOrder(int newSortOrder)
    {
        SortOrder = newSortOrder;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Toggles the completion status of this task.
    /// 
    /// If the task is currently completed, it will be reopened.
    /// If the task is not completed, it will be marked as completed.
    /// This is a convenience method for simple completion toggling operations.
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
