using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class CollateralValuationConfiguration : IEntityTypeConfiguration<CollateralValuation>
{
    public void Configure(EntityTypeBuilder<CollateralValuation> builder)
    {
        builder.ToTable("CollateralValuations", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
