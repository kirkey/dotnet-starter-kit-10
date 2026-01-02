using FSH.Modules.Todo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Todo.Data.Configurations;

public class TodoTaskConfiguration : IEntityTypeConfiguration<TodoTask>
{
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

        // Indexes
        builder.HasIndex(t => t.TodoId);
        builder.HasIndex(t => t.TenantId);
        builder.HasIndex(t => t.SortOrder);
        builder.HasIndex(t => t.IsCompleted);
        builder.HasIndex(t => t.IsActive);
    }
}
