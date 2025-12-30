using FSH.Modules.Todo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Todo.Data.Configurations;

public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        // Table mapping
        builder.ToTable("TodoLists", TodoModuleConstants.SchemaName);

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

        builder.Property(x => x.Color)
            .HasMaxLength(50);

        builder.Property(x => x.CreatedByUserName)
            .HasMaxLength(200);

        builder.Property(x => x.LastModifiedByUserName)
            .HasMaxLength(200);

        // Indexes
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.CreatedOnUtc);

        // Relationships
        builder.HasMany(x => x.Items)
            .WithOne(x => x.TodoList)
            .HasForeignKey(x => x.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
