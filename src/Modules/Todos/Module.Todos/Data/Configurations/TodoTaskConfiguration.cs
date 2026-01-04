using FSH.Module.Todos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Todos.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the TodoTask entity.
/// 
/// **Purpose:**
/// Defines the database mapping, validation rules, and relationships for TodoTask entities.
/// Configured for the "todo" schema alongside the Todo entity.
/// Uses centralized string length constants from TodoStringLengths for consistency.
/// 
/// **Table Details:**
/// - Schema: "todo"
/// - Table: "TodoTasks"
/// - Primary Key: Id (Guid)
/// - Foreign Key: TodoId (references Todos.Id with cascade delete)
/// 
/// **Property Configuration:**
/// - Name: Required, max 128 chars (TodoStringLengths.TodoTaskNameMaxLength)
/// - Description: Optional, max 512 chars (TodoStringLengths.TodoTaskDescriptionMaxLength)
/// - Notes: Optional, max 2048 chars (TodoStringLengths.TodoTaskNotesMaxLength)
/// - Status: Required, max 32 chars (TodoStringLengths.TodoTaskStatusMaxLength)
/// - IsActive: Required, boolean (soft delete support)
/// - IsCompleted: Required, boolean
/// - SortOrder: Required, int (for task ordering)
/// - TenantId: Required for multi-tenancy, max 64 chars (TodoStringLengths.TodoTaskTenantIdMaxLength)
/// - CreatedByUserName: Optional, max 256 chars (TodoStringLengths.TodoTaskCreatedByUserNameMaxLength)
/// - LastModifiedByUserName: Optional, max 256 chars (TodoStringLengths.TodoTaskLastModifiedByUserNameMaxLength)
/// 
/// **Relationships:**
/// - Many-to-One with Todo (cascade delete on parent)
/// - When a Todo is deleted, all its tasks are automatically deleted
/// 
/// **Indexes:**
/// - TodoId: Foreign key lookup (find tasks for a todo)
/// - TenantId: Multi-tenancy queries
/// - SortOrder: Task ordering queries
/// - IsCompleted: Task completion status filtering
/// - IsActive: Active tasks filtering
/// 
/// **String Length Strategy:**
/// Uses power-of-2 byte lengths (128, 256, 512, 2048) for:
/// - Memory alignment efficiency
/// - Better index performance
/// - Industry standard practice
/// - Future-proofing (room for growth)
/// </summary>
public class TodoTaskConfiguration : IEntityTypeConfiguration<TodoTask>
{
    /// <summary>
    /// Configures the TodoTask entity mapping in the database.
    /// 
    /// All string length constraints are defined in TodoStringLengths class
    /// to ensure consistency across domain, validators, and database layers.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<TodoTask> builder)
    {
        builder.ToTable("TodoTasks", "todo");

        builder.HasKey(t => t.Id);

        // AuditableEntity fields - Name, Description, Notes, Status, IsActive
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(TodoStringLengths.TodoTaskNameMaxLength);

        builder.Property(t => t.Description)
            .HasMaxLength(TodoStringLengths.TodoTaskDescriptionMaxLength);

        builder.Property(t => t.Notes)
            .HasMaxLength(TodoStringLengths.TodoTaskNotesMaxLength);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasMaxLength(TodoStringLengths.TodoTaskStatusMaxLength);

        builder.Property(t => t.IsActive)
            .IsRequired();

        // TodoTask-specific fields
        builder.Property(t => t.IsCompleted)
            .IsRequired();

        builder.Property(t => t.SortOrder)
            .IsRequired();

        // Audit fields
        builder.Property(t => t.TenantId)
            .IsRequired()
            .HasMaxLength(TodoStringLengths.TodoTaskTenantIdMaxLength);

        builder.Property(t => t.CreatedByUserName)
            .HasMaxLength(TodoStringLengths.TodoTaskCreatedByUserNameMaxLength);

        builder.Property(t => t.LastModifiedByUserName)
            .HasMaxLength(TodoStringLengths.TodoTaskLastModifiedByUserNameMaxLength);

        // Relationship: TodoTask belongs to Todo
        builder.HasOne(t => t.Todo)
            .WithMany(todo => todo.Tasks)
            .HasForeignKey(t => t.TodoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for common query patterns
        builder.HasIndex(t => t.TodoId);
        builder.HasIndex(t => t.TenantId);
        builder.HasIndex(t => t.SortOrder);
        builder.HasIndex(t => t.IsCompleted);
        builder.HasIndex(t => t.IsActive);
    }
}
