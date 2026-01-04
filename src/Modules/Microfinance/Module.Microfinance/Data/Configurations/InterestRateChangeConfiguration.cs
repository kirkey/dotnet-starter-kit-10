using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class InterestRateChangeConfiguration : IEntityTypeConfiguration<InterestRateChange>
{
    public void Configure(EntityTypeBuilder<InterestRateChange> builder)
    {
        builder.ToTable("InterestRateChanges", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
