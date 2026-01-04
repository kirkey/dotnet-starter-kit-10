using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class CreditScoreConfiguration : IEntityTypeConfiguration<CreditScore>
{
    public void Configure(EntityTypeBuilder<CreditScore> builder)
    {
        builder.ToTable("CreditScores", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
