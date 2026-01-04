using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class CollateralTypeConfiguration : IEntityTypeConfiguration<CollateralType>
{
    public void Configure(EntityTypeBuilder<CollateralType> builder)
    {
        builder.ToTable("CollateralTypes", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
