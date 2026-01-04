using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class TellerSessionConfiguration : IEntityTypeConfiguration<TellerSession>
{
    public void Configure(EntityTypeBuilder<TellerSession> builder)
    {
        builder.ToTable("TellerSessions", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
