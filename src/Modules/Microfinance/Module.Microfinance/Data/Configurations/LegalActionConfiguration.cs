using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class LegalActionConfiguration : IEntityTypeConfiguration<LegalAction>
{
    public void Configure(EntityTypeBuilder<LegalAction> builder)
    {
        builder.ToTable("LegalActions", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
