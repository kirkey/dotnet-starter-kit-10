using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class AmlAlertConfiguration : IEntityTypeConfiguration<AmlAlert>
{
    public void Configure(EntityTypeBuilder<AmlAlert> builder)
    {
        builder.ToTable("AmlAlerts", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
