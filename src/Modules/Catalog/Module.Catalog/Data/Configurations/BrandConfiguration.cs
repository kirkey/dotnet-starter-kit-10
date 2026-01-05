using FSH.Module.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Catalog.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Brand entity.
/// </summary>
public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands", "catalog");

        builder.HasKey(b => b.Id);

        // Properties
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(CatalogStringLengths.BrandNameMaxLength);

        builder.Property(b => b.Description)
            .HasMaxLength(CatalogStringLengths.BrandDescriptionMaxLength);

        builder.Property(b => b.Website)
            .HasMaxLength(CatalogStringLengths.BrandWebsiteMaxLength);

        builder.Property(b => b.IsActive)
            .IsRequired();

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

        // Indexes
        builder.HasIndex(b => b.TenantId);
        builder.HasIndex(b => b.Name);
        builder.HasIndex(b => b.IsActive);
        builder.HasIndex(b => b.CreatedOnUtc);
    }
}
