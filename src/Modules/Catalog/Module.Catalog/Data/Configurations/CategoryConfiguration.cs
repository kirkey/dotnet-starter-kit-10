using FSH.Module.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Catalog.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Category entity.
/// 
/// **Purpose:**
/// Defines the database mapping, validation rules, and relationships for Category entities.
/// Configured for the "catalog" schema to provide logical separation from other modules.
/// Uses centralized string length constants from CatalogStringLengths for consistency.
/// 
/// **Table Details:**
/// - Schema: "catalog"
/// - Table: "Categories"
/// - Primary Key: Id (Guid)
/// 
/// **Property Configuration:**
/// - Name: Required, max 128 chars
/// - Code: Required, max 64 chars, unique
/// - Description: Optional, max 512 chars
/// - ParentId: Optional (for hierarchical categories)
/// - TenantId: Required for multi-tenancy, max 64 chars
/// 
/// **Relationships:**
/// - Self-referencing: Parent-Child categories (optional)
/// - One-to-Many with Product
/// 
/// **Indexes:**
/// - TenantId: Multi-tenancy queries
/// - Code: Unique category codes
/// - ParentId: Hierarchical queries
/// - IsActive: Active categories filtering
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", "catalog");

        builder.HasKey(c => c.Id);

        // AuditableEntity fields
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.CategoryNameMaxLength);

        builder.Property(c => c.Description)
            .HasMaxLength(CatalogStringLengths.CategoryDescriptionMaxLength);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(c => c.IsActive)
            .IsRequired();

        // Category-specific fields
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.CategoryCodeMaxLength);

        // Audit fields
        builder.Property(c => c.TenantId)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.CategoryTenantIdMaxLength);

        builder.Property(c => c.CreatedByUserName)
            .HasMaxLength(CatalogStringLengths.CategoryCreatedByUserNameMaxLength);

        builder.Property(c => c.LastModifiedByUserName)
            .HasMaxLength(CatalogStringLengths.CategoryLastModifiedByUserNameMaxLength);

        // Self-referencing relationship: Parent-Child categories
        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship: Category has many Products
        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for common query patterns
        builder.HasIndex(c => c.TenantId);
        builder.HasIndex(c => new { c.TenantId, c.Code }).IsUnique();
        builder.HasIndex(c => c.ParentId);
        builder.HasIndex(c => c.IsActive);
    }
}
