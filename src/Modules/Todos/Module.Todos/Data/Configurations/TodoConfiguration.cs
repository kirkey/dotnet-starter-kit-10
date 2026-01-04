using FSH.Module.Todos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Todos.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Todo entity.
/// 
/// **Purpose:**
/// Defines the database mapping, validation rules, and relationships for Todo entities.
/// Configured for the "todo" schema to provide logical separation from other modules.
/// Uses centralized string length constants from TodoStringLengths for consistency.
/// 
/// **Table Details:**
/// - Schema: "todo"
/// - Table: "Todos"
/// - Primary Key: Id (Guid)
/// 
/// **Property Configuration:**
/// - Name: Required, max 128 chars (TodoStringLengths.TodoNameMaxLength)
/// - Description: Optional, max 512 chars (TodoStringLengths.TodoDescriptionMaxLength)
/// - Notes: Optional, max 2048 chars (TodoStringLengths.TodoNotesMaxLength)
/// - Status: Required, max 32 chars (TodoStringLengths.TodoStatusMaxLength - enum string)
/// - Priority: Required, stored as int (enum conversion)
/// - IsActive: Required, boolean (soft delete support)
/// - DueDate: Optional, DateTimeOffset
/// - IsCompleted: Required, boolean
/// - TenantId: Required for multi-tenancy, max 64 chars (TodoStringLengths.TodoTenantIdMaxLength)
/// - CreatedByUserName: Optional, max 256 chars (TodoStringLengths.TodoCreatedByUserNameMaxLength)
/// - LastModifiedByUserName: Optional, max 256 chars (TodoStringLengths.TodoLastModifiedByUserNameMaxLength)
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
/// 
/// **String Length Strategy:**
/// Uses power-of-2 byte lengths (128, 256, 512, 2048) for:
/// - Memory alignment efficiency
/// - Better index performance
/// - Industry standard practice
/// - Future-proofing (room for growth)
/// </summary>
public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    /// <summary>
    /// Configures the Todo entity mapping in the database.
    /// 
    /// All string length constraints are defined in TodoStringLengths class
    /// to ensure consistency across domain, validators, and database layers.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("Todos", "todo");

        builder.HasKey(t => t.Id);

        // AuditableEntity fields - Name, Description, Notes, Status, IsActive
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(TodoStringLengths.TodoNameMaxLength);

        builder.Property(t => t.Description)
            .HasMaxLength(TodoStringLengths.TodoDescriptionMaxLength);

        builder.Property(t => t.Notes)
            .HasMaxLength(TodoStringLengths.TodoNotesMaxLength);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasMaxLength(TodoStringLengths.TodoStatusMaxLength);

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
            .HasMaxLength(TodoStringLengths.TodoTenantIdMaxLength);

        builder.Property(t => t.CreatedByUserName)
            .HasMaxLength(TodoStringLengths.TodoCreatedByUserNameMaxLength);

        builder.Property(t => t.LastModifiedByUserName)
            .HasMaxLength(TodoStringLengths.TodoLastModifiedByUserNameMaxLength);

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
