using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class CollectionCaseConfiguration : IEntityTypeConfiguration<CollectionCase>
{
    public void Configure(EntityTypeBuilder<CollectionCase> builder)
    {
        builder.ToTable("CollectionCases", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
