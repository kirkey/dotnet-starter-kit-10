using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class FeeChargeConfiguration : IEntityTypeConfiguration<FeeCharge>
{
    public void Configure(EntityTypeBuilder<FeeCharge> builder)
    {
        builder.ToTable("FeeCharges", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
