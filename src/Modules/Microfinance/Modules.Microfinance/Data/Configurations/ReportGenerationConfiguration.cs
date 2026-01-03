using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class ReportGenerationConfiguration : IEntityTypeConfiguration<ReportGeneration>
{
    public void Configure(EntityTypeBuilder<ReportGeneration> builder)
    {
        builder.ToTable("ReportGenerations", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
