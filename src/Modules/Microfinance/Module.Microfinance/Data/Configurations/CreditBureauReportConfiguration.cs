using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class CreditBureauReportConfiguration : IEntityTypeConfiguration<CreditBureauReport>
{
    public void Configure(EntityTypeBuilder<CreditBureauReport> builder)
    {
        builder.ToTable("CreditBureauReports", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
