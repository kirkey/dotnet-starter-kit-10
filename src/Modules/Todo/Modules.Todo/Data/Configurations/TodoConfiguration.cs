using FSH.Framework.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoEntity = FSH.Modules.Todo.Domain.Todo;

namespace FSH.Modules.Todo.Data.Configurations;

public class TodoConfiguration : IEntityTypeConfiguration<TodoEntity>
{
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

        // Indexes
        builder.HasIndex(t => t.TenantId);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Priority);
        builder.HasIndex(t => t.DueDate);
        builder.HasIndex(t => t.CreatedOnUtc);
        builder.HasIndex(t => t.IsActive);
    }
}
