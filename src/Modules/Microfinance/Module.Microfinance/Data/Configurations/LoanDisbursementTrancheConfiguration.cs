using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class LoanDisbursementTrancheConfiguration : IEntityTypeConfiguration<LoanDisbursementTranche>
{
    public void Configure(EntityTypeBuilder<LoanDisbursementTranche> builder)
    {
        builder.ToTable("LoanDisbursementTranches", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
