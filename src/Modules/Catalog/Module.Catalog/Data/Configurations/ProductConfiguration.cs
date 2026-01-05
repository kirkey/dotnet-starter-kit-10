using FSH.Module.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Catalog.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Product entity.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", "catalog");

        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.ProductNameMaxLength);

        builder.Property(p => p.Description)
            .HasMaxLength(CatalogStringLengths.ProductDescriptionMaxLength);

        builder.Property(p => p.SKU)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.ProductSKUMaxLength);

        builder.Property(p => p.Barcode)
            .HasMaxLength(CatalogStringLengths.ProductBarcodeMaxLength);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Cost)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.QuantityInStock)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.ProductStatusMaxLength);

        builder.Property(p => p.IsActive)
            .IsRequired();

        // Audit fields
        builder.Property(p => p.TenantId)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.ProductTenantIdMaxLength);

        builder.Property(p => p.CreatedByUserName)
            .HasMaxLength(CatalogStringLengths.ProductCreatedByUserNameMaxLength);

        builder.Property(p => p.LastModifiedByUserName)
            .HasMaxLength(CatalogStringLengths.ProductLastModifiedByUserNameMaxLength);

        // Relationships configured in Category and Brand configurations

        // Indexes
        builder.HasIndex(p => p.TenantId);
        builder.HasIndex(p => p.SKU);
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.BrandId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.IsActive);
        builder.HasIndex(p => p.CreatedOnUtc);
    }
}
