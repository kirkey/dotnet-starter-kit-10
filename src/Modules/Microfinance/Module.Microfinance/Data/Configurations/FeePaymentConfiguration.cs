using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class FeePaymentConfiguration : IEntityTypeConfiguration<FeePayment>
{
    public void Configure(EntityTypeBuilder<FeePayment> builder)
    {
        builder.ToTable("FeePayments", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
