using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class QrPaymentConfiguration : IEntityTypeConfiguration<QrPayment>
{
    public void Configure(EntityTypeBuilder<QrPayment> builder)
    {
        builder.ToTable("QrPayments", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
