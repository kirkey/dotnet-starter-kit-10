using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class FeeWaiverConfiguration : IEntityTypeConfiguration<FeeWaiver>
{
    public void Configure(EntityTypeBuilder<FeeWaiver> builder)
    {
        builder.ToTable("FeeWaivers", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
