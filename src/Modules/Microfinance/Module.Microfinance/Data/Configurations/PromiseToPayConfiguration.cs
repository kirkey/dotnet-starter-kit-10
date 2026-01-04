using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class PromiseToPayConfiguration : IEntityTypeConfiguration<PromiseToPay>
{
    public void Configure(EntityTypeBuilder<PromiseToPay> builder)
    {
        builder.ToTable("PromiseToPays", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
