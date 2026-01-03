using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class InvestmentProductConfiguration : IEntityTypeConfiguration<InvestmentProduct>
{
    public void Configure(EntityTypeBuilder<InvestmentProduct> builder)
    {
        builder.ToTable("InvestmentProducts", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
