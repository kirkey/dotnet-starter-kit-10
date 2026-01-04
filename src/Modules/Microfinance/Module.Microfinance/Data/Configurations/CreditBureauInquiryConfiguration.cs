using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class CreditBureauInquiryConfiguration : IEntityTypeConfiguration<CreditBureauInquiry>
{
    public void Configure(EntityTypeBuilder<CreditBureauInquiry> builder)
    {
        builder.ToTable("CreditBureauInquirys", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
