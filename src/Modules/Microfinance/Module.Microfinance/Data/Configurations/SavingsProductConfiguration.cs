using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class SavingsProductConfiguration : IEntityTypeConfiguration<SavingsProduct>
{
    public void Configure(EntityTypeBuilder<SavingsProduct> builder)
    {
        builder.ToTable("SavingsProducts", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
