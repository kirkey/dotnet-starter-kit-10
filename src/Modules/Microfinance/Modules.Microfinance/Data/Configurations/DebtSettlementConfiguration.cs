using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class DebtSettlementConfiguration : IEntityTypeConfiguration<DebtSettlement>
{
    public void Configure(EntityTypeBuilder<DebtSettlement> builder)
    {
        builder.ToTable("DebtSettlements", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
