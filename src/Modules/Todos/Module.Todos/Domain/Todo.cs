using FSH.Framework.Core.Domain;

namespace FSH.Module.Todos.Domain;

/// <summary>
/// Master entity representing a Todo item with status and priority management.
/// 
/// **Purpose:**
/// Serves as the root aggregate for the Todo domain model. Manages the lifecycle of a todo item,
/// including creation, updates, completion tracking, and archival. Maintains a collection of
/// TodoTask items that represent sub-tasks within the todo.
/// 
/// **Business Logic:**
/// - Todos can be in one of four statuses: NotStarted, InProgress, Completed, OnHold
/// - Each todo has a priority level that influences execution order
/// - Completion tracking includes both a boolean flag and a timestamp
/// - Todos can be archived without deletion (soft delete pattern)
/// - Todos can be reopened after completion
/// - Status changes to Completed automatically mark the todo as completed and record the timestamp
/// 
/// **Relationships:**
/// - Master in a one-to-many relationship with TodoTask entities
/// - Inherits audit tracking (CreatedBy, CreatedOnUtc, LastModifiedBy, LastModifiedOnUtc)
/// 
/// **Multi-Tenancy:**
/// - Supports multi-tenancy through TenantId property
/// 
/// **Audit Trail:**
/// - Tracks creation details (CreatedBy, CreatedByUserName, CreatedOnUtc)
/// - Tracks modification details (LastModifiedBy, LastModifiedByUserName, LastModifiedOnUtc)
/// - Supports soft deletes through IsActive flag
/// </summary>
public class Todo : AuditableEntity<Guid>
{
    /// <summary>
    /// Gets or sets the priority level of this todo.
    /// Defaults to Medium priority.
    /// </summary>
    public TodoPriority Priority { get; private set; } = TodoPriority.Medium;
    
    /// <summary>
    /// Gets or sets the due date for this todo (nullable).
    /// Stored in UTC for consistency across timezones.
    /// </summary>
    public DateTimeOffset? DueDate { get; private set; }
    
    /// <summary>
    /// Gets a value indicating whether this todo has been completed.
    /// </summary>
    public bool IsCompleted { get; private set; }
    
    /// <summary>
    /// Gets the timestamp when this todo was completed (nullable).
    /// Only set when IsCompleted is true.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; private set; }

    /// <summary>
    /// Gets the collection of TodoTask items associated with this todo.
    /// Represents the detail side of the master-detail relationship.
    /// </summary>
    public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();

    private Todo() { } // EF Core

    /// <summary>
    /// Factory method to create a new Todo item.
    /// 
    /// Initializes a new todo with the provided details and sets default values for:
    /// - Status: NotStarted
    /// - IsActive: true
    /// - IsCompleted: false
    /// - Priority: Medium (if not specified)
    /// </summary>
    /// <param name="name">The name/title of the todo (required, cannot be null or whitespace).</param>
    /// <param name="tenantId">The tenant ID for multi-tenancy support (required).</param>
    /// <param name="createdBy">The user ID of the person creating the todo.</param>
    /// <param name="createdByUserName">The username of the person creating the todo.</param>
    /// <param name="description">Optional description providing more details about the todo.</param>
    /// <param name="priority">The priority level of the todo (defaults to Medium).</param>
    /// <param name="dueDate">Optional due date for the todo. Will be converted to UTC.</param>
    /// <returns>A new Todo instance with all properties initialized.</returns>
    /// <exception cref="ArgumentException">Thrown when name or tenantId is null or whitespace.</exception>
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
            // Convert to UTC to ensure PostgreSQL compatibility
            DueDate = dueDate?.ToUniversalTime(),
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            Status = nameof(TodoStatus.NotStarted),
            IsActive = true,
            IsCompleted = false
        };
    }

    /// <summary>
    /// Updates the details of this todo.
    /// 
    /// Updates the name, description, priority, and due date of the todo.
    /// Automatically records the modification details.
    /// </summary>
    /// <param name="name">The new name/title (required, cannot be null or whitespace).</param>
    /// <param name="description">The new description (optional).</param>
    /// <param name="priority">The new priority level.</param>
    /// <param name="dueDate">The new due date. Will be converted to UTC. Can be null.</param>
    /// <param name="modifiedBy">The user ID of the person making the modification.</param>
    /// <param name="modifiedByUserName">The username of the person making the modification.</param>
    /// <exception cref="ArgumentException">Thrown when name is null or whitespace.</exception>
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
        // Convert to UTC to ensure PostgreSQL compatibility
        DueDate = dueDate?.ToUniversalTime();
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }

    /// <summary>
    /// Marks this todo as completed.
    /// 
    /// Sets IsCompleted to true, records the completion timestamp, and updates the status to Completed.
    /// This is typically called when the todo work is finished.
    /// </summary>
    public void Complete()
    {
        IsCompleted = true;
        CompletedAt = DateTimeOffset.UtcNow;
        Status = nameof(TodoStatus.Completed);
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Reopens a previously completed todo.
    /// 
    /// Sets IsCompleted to false, clears the completion timestamp, and updates the status to InProgress.
    /// Use this when a completed todo needs to be worked on again.
    /// </summary>
    public void Reopen()
    {
        IsCompleted = false;
        CompletedAt = null;
        Status = nameof(TodoStatus.InProgress);
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Updates the status of this todo.
    /// 
    /// Changes the todo's status to the specified value. If the new status is Completed and the todo
    /// is not already marked as completed, automatically marks it as completed with a timestamp.
    /// </summary>
    /// <param name="status">The new status for the todo.</param>
    public void UpdateStatus(TodoStatus status)
    {
        Status = status.ToString();
        LastModifiedOnUtc = DateTimeOffset.UtcNow;

        if (status == TodoStatus.Completed && !IsCompleted)
        {
            Complete();
        }
    }

    /// <summary>
    /// Archives this todo (soft delete).
    /// 
    /// Sets IsActive to false, marking the todo as archived without permanent deletion.
    /// Archived todos can be restored using the Restore method.
    /// </summary>
    public void Archive()
    {
        IsActive = false;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Restores a previously archived todo.
    /// 
    /// Sets IsActive to true, making an archived todo active again.
    /// </summary>
    public void Restore()
    {
        IsActive = true;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
}
