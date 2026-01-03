using FSH.Framework.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoEntity = FSH.Modules.Todo.Domain.Todo;

namespace FSH.Modules.Todo.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Todo entity.
/// 
/// **Purpose:**
/// Defines the database mapping, validation rules, and relationships for Todo entities.
/// Configured for the "todo" schema to provide logical separation from other modules.
/// 
/// **Table Details:**
/// - Schema: "todo"
/// - Table: "Todos"
/// - Primary Key: Id (Guid)
/// 
/// **Property Configuration:**
/// - Name: Required, max 200 chars
/// - Description: Optional, max 2000 chars
/// - Notes: Optional, max 2000 chars
/// - Status: Required, max 50 chars (enum string conversion)
/// - Priority: Required, stored as int (enum conversion)
/// - IsActive: Required, boolean (soft delete support)
/// - DueDate: Optional, DateTimeOffset
/// - IsCompleted: Required, boolean
/// - TenantId: Required for multi-tenancy, max 64 chars
/// - CreatedByUserName: Optional, max 256 chars
/// - LastModifiedByUserName: Optional, max 256 chars
/// 
/// **Relationships:**
/// - One-to-Many with TodoTask (cascade delete)
/// 
/// **Indexes:**
/// - TenantId: Multi-tenancy queries
/// - Status: Status filtering
/// - Priority: Priority-based queries
/// - DueDate: Due date sorting/filtering
/// - CreatedOnUtc: Creation date sorting
/// - IsActive: Active todos filtering
/// </summary>
public class TodoConfiguration : IEntityTypeConfiguration<TodoEntity>
{
    /// <summary>
    /// Configures the Todo entity mapping in the database.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<TodoEntity> builder)
    {
        builder.ToTable("Todos", "todo");

        builder.HasKey(t => t.Id);

        // AuditableEntity fields - Name, Description, Notes, Status, IsActive
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(2000);

        builder.Property(t => t.Notes)
            .HasMaxLength(2000);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.IsActive)
            .IsRequired();

        // Todo-specific fields
        builder.Property(t => t.Priority)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.IsCompleted)
            .IsRequired();

        // Audit fields
        builder.Property(t => t.TenantId)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(t => t.CreatedByUserName)
            .HasMaxLength(256);

        builder.Property(t => t.LastModifiedByUserName)
            .HasMaxLength(256);

        // Relationship: Todo has many Tasks
        builder.HasMany(t => t.Tasks)
            .WithOne(task => task.Todo)
            .HasForeignKey(task => task.TodoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for common query patterns
        builder.HasIndex(t => t.TenantId);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Priority);
        builder.HasIndex(t => t.DueDate);
        builder.HasIndex(t => t.CreatedOnUtc);
        builder.HasIndex(t => t.IsActive);
    }
}
