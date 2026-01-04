using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class FeeDefinitionConfiguration : IEntityTypeConfiguration<FeeDefinition>
{
    public void Configure(EntityTypeBuilder<FeeDefinition> builder)
    {
        builder.ToTable("FeeDefinitions", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
