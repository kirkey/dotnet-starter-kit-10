using FSH.Module.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Accounting.Data.Configurations;

public class FixedAssetConfiguration : IEntityTypeConfiguration<FixedAsset>
{
    public void Configure(EntityTypeBuilder<FixedAsset> builder)
    {
        builder.ToTable("FixedAssets", "accounting");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(AccountingStringLengths.Name);
        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.AcquisitionDate);
        builder.Property(x => x.Cost).HasPrecision(18, 2);
        builder.Property(x => x.ResidualValue).HasPrecision(18, 2);
        builder.Property(x => x.DepreciationRate).HasPrecision(8, 6);
        builder.Property(x => x.AccumulatedDepreciation).HasPrecision(18, 2);
        builder.Property(x => x.IsDisposed).IsRequired();
        builder.Property(x => x.DisposalDate);
        builder.Property(x => x.DisposalProceeds).HasPrecision(18, 2);
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.Name });
        builder.HasIndex(x => x.IsDisposed);
        builder.HasIndex(x => x.AcquisitionDate);
    }
}
