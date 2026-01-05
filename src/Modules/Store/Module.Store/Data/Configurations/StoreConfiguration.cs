using FSH.Module.Store.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Store.Data.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable("Stores", "store");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).IsRequired().HasMaxLength(StoreStringLengths.StoreNameMaxLength);
        builder.Property(s => s.Description).HasMaxLength(StoreStringLengths.StoreDescriptionMaxLength);
        builder.Property(s => s.Address).IsRequired().HasMaxLength(StoreStringLengths.StoreAddressMaxLength);
        builder.Property(s => s.City).IsRequired().HasMaxLength(StoreStringLengths.StoreCityMaxLength);
        builder.Property(s => s.State).HasMaxLength(StoreStringLengths.StoreStateMaxLength);
        builder.Property(s => s.PostalCode).IsRequired().HasMaxLength(StoreStringLengths.StorePostalCodeMaxLength);
        builder.Property(s => s.Phone).HasMaxLength(StoreStringLengths.StorePhoneMaxLength);
        builder.Property(s => s.Email).HasMaxLength(StoreStringLengths.StoreEmailMaxLength);
        builder.Property(s => s.Status).IsRequired().HasMaxLength(64);
        builder.Property(s => s.IsActive).IsRequired();
        builder.Property(s => s.TenantId).IsRequired().HasMaxLength(StoreStringLengths.StoreTenantIdMaxLength);
        builder.Property(s => s.CreatedByUserName).HasMaxLength(StoreStringLengths.StoreCreatedByUserNameMaxLength);
        builder.Property(s => s.LastModifiedByUserName).HasMaxLength(StoreStringLengths.StoreLastModifiedByUserNameMaxLength);

        builder.HasMany(s => s.POSTerminals).WithOne(p => p.Store).HasForeignKey(p => p.StoreId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.TenantId);
        builder.HasIndex(s => s.IsActive);
    }
}
