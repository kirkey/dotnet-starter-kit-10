using FSH.Modules.Todo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Todo.Data.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        // Table mapping
        builder.ToTable("TodoItems", TodoModuleConstants.SchemaName);

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Priority)
            .IsRequired();

        builder.Property(x => x.AssignedToUserId)
            .HasMaxLength(200);

        builder.Property(x => x.AssignedToUserName)
            .HasMaxLength(200);

        builder.Property(x => x.CreatedByUserName)
            .HasMaxLength(200);

        builder.Property(x => x.LastModifiedByUserName)
            .HasMaxLength(200);

        // Indexes
        builder.HasIndex(x => x.TodoListId);
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.Priority);
        builder.HasIndex(x => x.DueDate);
        builder.HasIndex(x => x.AssignedToUserId);
        builder.HasIndex(x => x.CreatedOnUtc);
    }
}
