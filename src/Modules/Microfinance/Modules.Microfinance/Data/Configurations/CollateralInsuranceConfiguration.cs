using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class CollateralInsuranceConfiguration : IEntityTypeConfiguration<CollateralInsurance>
{
    public void Configure(EntityTypeBuilder<CollateralInsurance> builder)
    {
        builder.ToTable("CollateralInsurances", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
