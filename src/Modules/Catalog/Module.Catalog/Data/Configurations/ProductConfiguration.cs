using FSH.Module.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Catalog.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Product entity.
/// 
/// **Purpose:**
/// Defines the database mapping, validation rules, and relationships for Product entities.
/// Configured for the "catalog" schema to provide logical separation from other modules.
/// Uses centralized string length constants from CatalogStringLengths for consistency.
/// 
/// **Table Details:**
/// - Schema: "catalog"
/// - Table: "Products"
/// - Primary Key: Id (Guid)
/// 
/// **Property Configuration:**
/// - Name: Required, max 256 chars
/// - SKU: Required, max 64 chars, unique
/// - Description: Optional, max 1024 chars
/// - Barcode: Optional, max 128 chars
/// - Price: Required, decimal(18,2)
/// - Cost: Optional, decimal(18,2)
/// - QuantityInStock: Required, integer
/// - Specifications: Optional, max 2048 chars
/// - CategoryId: Required (FK to Category)
/// - BrandId: Required (FK to Brand)
/// - TenantId: Required for multi-tenancy, max 64 chars
/// 
/// **Relationships:**
/// - Many-to-One with Category
/// - Many-to-One with Brand
/// 
/// **Indexes:**
/// - TenantId: Multi-tenancy queries
/// - SKU: Unique product SKUs
/// - CategoryId: Category-based queries
/// - BrandId: Brand-based queries
/// - Barcode: Barcode scanning
/// - IsActive: Active products filtering
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", "catalog");

        builder.HasKey(p => p.Id);

        // AuditableEntity fields
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.ProductNameMaxLength);

        builder.Property(p => p.Description)
            .HasMaxLength(CatalogStringLengths.ProductDescriptionMaxLength);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(p => p.IsActive)
            .IsRequired();

        // Product-specific fields
        builder.Property(p => p.SKU)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.ProductSKUMaxLength);

        builder.Property(p => p.Barcode)
            .HasMaxLength(CatalogStringLengths.ProductBarcodeMaxLength);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.Cost)
            .HasPrecision(18, 2);

        builder.Property(p => p.QuantityInStock)
            .IsRequired();

        builder.Property(p => p.Specifications)
            .HasMaxLength(CatalogStringLengths.ProductSpecificationsMaxLength);

        // Foreign keys
        builder.Property(p => p.CategoryId)
            .IsRequired();

        builder.Property(p => p.BrandId)
            .IsRequired();

        // Audit fields
        builder.Property(p => p.TenantId)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.ProductTenantIdMaxLength);

        builder.Property(p => p.CreatedByUserName)
            .HasMaxLength(CatalogStringLengths.ProductCreatedByUserNameMaxLength);

        builder.Property(p => p.LastModifiedByUserName)
            .HasMaxLength(CatalogStringLengths.ProductLastModifiedByUserNameMaxLength);

        // Relationships are defined in Category and Brand configurations

        // Indexes for common query patterns
        builder.HasIndex(p => p.TenantId);
        builder.HasIndex(p => new { p.TenantId, p.SKU }).IsUnique();
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.BrandId);
        builder.HasIndex(p => p.Barcode);
        builder.HasIndex(p => p.IsActive);
        builder.HasIndex(p => p.Name);
    }
}
