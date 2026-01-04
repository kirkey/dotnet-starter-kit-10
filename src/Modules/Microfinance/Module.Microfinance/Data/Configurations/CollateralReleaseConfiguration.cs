using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class CollateralReleaseConfiguration : IEntityTypeConfiguration<CollateralRelease>
{
    public void Configure(EntityTypeBuilder<CollateralRelease> builder)
    {
        builder.ToTable("CollateralReleases", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
