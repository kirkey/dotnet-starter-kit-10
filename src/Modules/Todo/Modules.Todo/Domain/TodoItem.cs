using FSH.Framework.Core.Domain;

namespace FSH.Modules.Todo.Domain;

/// <summary>
/// Represents an item in a Todo List (Detail entity in master-detail relationship).
/// </summary>
public class TodoItem : BaseEntity<Guid>, IAuditableEntity
{
    // IAuditableEntity properties
    public DateTimeOffset CreatedOnUtc { get; set; } = DateTimeOffset.UtcNow;
    public Guid? CreatedBy { get; set; }
    public string? CreatedByUserName { get; set; }
    public DateTimeOffset? LastModifiedOnUtc { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public string? LastModifiedByUserName { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = TodoItemStatus.Pending;
    public bool IsActive { get; set; } = true;

    // Master reference
    public Guid TodoListId { get; set; }
    public virtual TodoList TodoList { get; set; } = default!;

    // Business Properties
    public int SortOrder { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public DateTimeOffset? CompletedDate { get; set; }
    public TodoPriority Priority { get; set; } = TodoPriority.Medium;
    public int? EstimatedHours { get; set; }
    public int? ActualHours { get; set; }
    public string? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }

    private TodoItem() { } // EF Core

    public static TodoItem Create(
        string name,
        Guid todoListId,
        Guid? createdBy,
        string? createdByUserName,
        string? description = null,
        TodoPriority priority = TodoPriority.Medium)
    {
        return new TodoItem
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            TodoListId = todoListId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            Priority = priority,
            Status = TodoItemStatus.Pending,
            IsActive = true
        };
    }

    public void Update(
        string name,
        Guid? modifiedBy,
        string? modifiedByUserName,
        string? description = null,
        TodoPriority? priority = null,
        string? status = null,
        bool? isActive = null)
    {
        Name = name;
        Description = description;
        if (priority.HasValue) Priority = priority.Value;
        if (status != null) Status = status;
        if (isActive.HasValue) IsActive = isActive.Value;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
        LastModifiedByUserName = modifiedByUserName;
    }

    public void Complete(Guid? completedBy, string? completedByUserName)
    {
        Status = TodoItemStatus.Completed;
        CompletedDate = DateTimeOffset.UtcNow;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
        LastModifiedBy = completedBy;
        LastModifiedByUserName = completedByUserName;
    }

    public void Assign(string userId, string userName, Guid? assignedBy, string? assignedByUserName)
    {
        AssignedToUserId = userId;
        AssignedToUserName = userName;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
        LastModifiedBy = assignedBy;
        LastModifiedByUserName = assignedByUserName;
    }
}
