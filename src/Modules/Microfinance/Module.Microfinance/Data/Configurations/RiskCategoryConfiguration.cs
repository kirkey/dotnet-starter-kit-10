using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class RiskCategoryConfiguration : IEntityTypeConfiguration<RiskCategory>
{
    public void Configure(EntityTypeBuilder<RiskCategory> builder)
    {
        builder.ToTable("RiskCategorys", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
