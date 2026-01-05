using FSH.Module.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Catalog.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Brand entity.
/// 
/// **Purpose:**
/// Defines the database mapping, validation rules, and relationships for Brand entities.
/// Configured for the "catalog" schema to provide logical separation from other modules.
/// Uses centralized string length constants from CatalogStringLengths for consistency.
/// 
/// **Table Details:**
/// - Schema: "catalog"
/// - Table: "Brands"
/// - Primary Key: Id (Guid)
/// 
/// **Property Configuration:**
/// - Name: Required, max 128 chars
/// - Description: Optional, max 512 chars
/// - WebsiteUrl: Optional, max 256 chars
/// - TenantId: Required for multi-tenancy, max 64 chars
/// 
/// **Relationships:**
/// - One-to-Many with Product
/// 
/// **Indexes:**
/// - TenantId: Multi-tenancy queries
/// - Name: Brand name searches
/// - IsActive: Active brands filtering
/// </summary>
public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands", "catalog");

        builder.HasKey(b => b.Id);

        // AuditableEntity fields
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.BrandNameMaxLength);

        builder.Property(b => b.Description)
            .HasMaxLength(CatalogStringLengths.BrandDescriptionMaxLength);

        builder.Property(b => b.Status)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(b => b.IsActive)
            .IsRequired();

        // Brand-specific fields
        builder.Property(b => b.WebsiteUrl)
            .HasMaxLength(CatalogStringLengths.BrandWebsiteUrlMaxLength);

        // Audit fields
        builder.Property(b => b.TenantId)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.BrandTenantIdMaxLength);

        builder.Property(b => b.CreatedByUserName)
            .HasMaxLength(CatalogStringLengths.BrandCreatedByUserNameMaxLength);

        builder.Property(b => b.LastModifiedByUserName)
            .HasMaxLength(CatalogStringLengths.BrandLastModifiedByUserNameMaxLength);

        // Relationship: Brand has many Products
        builder.HasMany(b => b.Products)
            .WithOne(p => p.Brand)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for common query patterns
        builder.HasIndex(b => b.TenantId);
        builder.HasIndex(b => b.Name);
        builder.HasIndex(b => b.IsActive);
    }
}
