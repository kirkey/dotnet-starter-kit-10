using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class RiskIndicatorConfiguration : IEntityTypeConfiguration<RiskIndicator>
{
    public void Configure(EntityTypeBuilder<RiskIndicator> builder)
    {
        builder.ToTable("RiskIndicators", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
