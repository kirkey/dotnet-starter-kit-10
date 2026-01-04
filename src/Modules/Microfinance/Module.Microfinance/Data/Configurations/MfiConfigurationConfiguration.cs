using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class MfiConfigurationConfiguration : IEntityTypeConfiguration<MfiConfiguration>
{
    public void Configure(EntityTypeBuilder<MfiConfiguration> builder)
    {
        builder.ToTable("MfiConfigurations", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
