using FSH.Framework.Core.Domain;
using FSH.Framework.Shared.Persistence;

namespace FSH.Modules.Todo.Domain;

/// <summary>
/// Represents a Todo List (Master entity in master-detail relationship).
/// </summary>
public class TodoList : BaseEntity<Guid>, IAuditableEntity, IHasTenant
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
    public string Status { get; set; } = TodoStatus.Active;
    public bool IsActive { get; set; } = true;

    // Multi-Tenancy
    public string TenantId { get; set; } = default!;

    // Business Properties
    public string? Color { get; set; }
    public int SortOrder { get; set; }
    public DateTimeOffset? DueDate { get; set; }

    // Navigation - Detail relationship
    public virtual ICollection<TodoItem> Items { get; set; } = new List<TodoItem>();

    private TodoList() { } // EF Core

    public static TodoList Create(
        string name,
        string tenantId,
        Guid? createdBy,
        string? createdByUserName,
        string? description = null,
        string? color = null)
    {
        return new TodoList
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            Color = color,
            Status = TodoStatus.Active,
            IsActive = true
        };
    }

    public void Update(
        string name,
        Guid? modifiedBy,
        string? modifiedByUserName,
        string? description = null,
        string? color = null,
        string? status = null,
        bool? isActive = null)
    {
        Name = name;
        Description = description;
        Color = color;
        if (status != null) Status = status;
        if (isActive.HasValue) IsActive = isActive.Value;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
        LastModifiedByUserName = modifiedByUserName;
    }

    public void AddItem(TodoItem item)
    {
        Items.Add(item);
    }

    public void RemoveItem(TodoItem item)
    {
        Items.Remove(item);
    }
}
