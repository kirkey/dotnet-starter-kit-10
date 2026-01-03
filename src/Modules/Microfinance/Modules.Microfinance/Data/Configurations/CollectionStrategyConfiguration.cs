using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class CollectionStrategyConfiguration : IEntityTypeConfiguration<CollectionStrategy>
{
    public void Configure(EntityTypeBuilder<CollectionStrategy> builder)
    {
        builder.ToTable("CollectionStrategys", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
