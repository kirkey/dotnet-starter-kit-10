using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class LoanCollateralConfiguration : IEntityTypeConfiguration<LoanCollateral>
{
    public void Configure(EntityTypeBuilder<LoanCollateral> builder)
    {
        builder.ToTable("LoanCollaterals", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
