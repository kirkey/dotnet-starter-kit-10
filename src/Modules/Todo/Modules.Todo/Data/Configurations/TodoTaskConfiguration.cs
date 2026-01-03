using FSH.Modules.Todo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Todo.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the TodoTask entity.
/// 
/// **Purpose:**
/// Defines the database mapping, validation rules, and relationships for TodoTask entities.
/// Configured for the "todo" schema alongside the Todo entity.
/// 
/// **Table Details:**
/// - Schema: "todo"
/// - Table: "TodoTasks"
/// - Primary Key: Id (Guid)
/// - Foreign Key: TodoId (references Todos.Id with cascade delete)
/// 
/// **Property Configuration:**
/// - Name: Required, max 200 chars
/// - Description: Optional, max 1000 chars
/// - Notes: Optional, max 1000 chars (inherited from AuditableEntity)
/// - Status: Required, max 50 chars (e.g., "Pending", "Completed")
/// - IsActive: Required, boolean (soft delete support)
/// - IsCompleted: Required, boolean
/// - SortOrder: Required, int (for task ordering)
/// - TenantId: Required for multi-tenancy, max 64 chars
/// - CreatedByUserName: Optional, max 256 chars
/// - LastModifiedByUserName: Optional, max 256 chars
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
/// </summary>
public class TodoTaskConfiguration : IEntityTypeConfiguration<TodoTask>
{
    /// <summary>
    /// Configures the TodoTask entity mapping in the database.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<TodoTask> builder)
    {
        builder.ToTable("TodoTasks", "todo");

        builder.HasKey(t => t.Id);

        // AuditableEntity fields - Name, Description, Notes, Status, IsActive
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.Notes)
            .HasMaxLength(1000);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasMaxLength(50);

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
            .HasMaxLength(64);

        builder.Property(t => t.CreatedByUserName)
            .HasMaxLength(256);

        builder.Property(t => t.LastModifiedByUserName)
            .HasMaxLength(256);

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
