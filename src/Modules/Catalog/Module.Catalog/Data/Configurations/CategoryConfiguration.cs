using FSH.Module.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Catalog.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Category entity.
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", "catalog");

        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.CategoryNameMaxLength);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.CategoryCodeMaxLength);

        builder.Property(c => c.Description)
            .HasMaxLength(CatalogStringLengths.CategoryDescriptionMaxLength);

        builder.Property(c => c.IsActive)
            .IsRequired();

        // Audit fields
        builder.Property(c => c.TenantId)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.CategoryTenantIdMaxLength);

        builder.Property(c => c.CreatedByUserName)
            .HasMaxLength(CatalogStringLengths.CategoryCreatedByUserNameMaxLength);

        builder.Property(c => c.LastModifiedByUserName)
            .HasMaxLength(CatalogStringLengths.CategoryLastModifiedByUserNameMaxLength);

        // Self-referential relationship for parent-child categories
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship: Category has many Products
        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(c => c.TenantId);
        builder.HasIndex(c => c.Code);
        builder.HasIndex(c => c.ParentCategoryId);
        builder.HasIndex(c => c.IsActive);
        builder.HasIndex(c => c.CreatedOnUtc);
    }
}
