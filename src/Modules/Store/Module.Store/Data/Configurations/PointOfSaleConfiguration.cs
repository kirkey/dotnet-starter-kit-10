using FSH.Module.Store.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Store.Data.Configurations;

public class PointOfSaleConfiguration : IEntityTypeConfiguration<PointOfSale>
{
    public void Configure(EntityTypeBuilder<PointOfSale> builder)
    {
        builder.ToTable("PointOfSales", "store");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(StoreStringLengths.POSNameMaxLength);
        builder.Property(p => p.Description).HasMaxLength(StoreStringLengths.POSDescriptionMaxLength);
        builder.Property(p => p.Identifier).IsRequired().HasMaxLength(StoreStringLengths.POSIdentifierMaxLength);
        builder.Property(p => p.Location).HasMaxLength(StoreStringLengths.POSLocationMaxLength);
        builder.Property(p => p.Status).IsRequired().HasMaxLength(64);
        builder.Property(p => p.IsActive).IsRequired();
        builder.Property(p => p.TenantId).IsRequired().HasMaxLength(StoreStringLengths.POSTenantIdMaxLength);
        builder.Property(p => p.CreatedByUserName).HasMaxLength(StoreStringLengths.POSCreatedByUserNameMaxLength);
        builder.Property(p => p.LastModifiedByUserName).HasMaxLength(StoreStringLengths.POSLastModifiedByUserNameMaxLength);

        builder.HasIndex(p => p.TenantId);
        builder.HasIndex(p => new { p.TenantId, p.Identifier }).IsUnique();
        builder.HasIndex(p => p.StoreId);
        builder.HasIndex(p => p.IsActive);
    }
}
