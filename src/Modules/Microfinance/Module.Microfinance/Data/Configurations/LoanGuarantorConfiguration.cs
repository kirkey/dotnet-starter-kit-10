using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class LoanGuarantorConfiguration : IEntityTypeConfiguration<LoanGuarantor>
{
    public void Configure(EntityTypeBuilder<LoanGuarantor> builder)
    {
        builder.ToTable("LoanGuarantors", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
