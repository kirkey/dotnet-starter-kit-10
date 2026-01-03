using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class AgentBankingConfiguration : IEntityTypeConfiguration<AgentBanking>
{
    public void Configure(EntityTypeBuilder<AgentBanking> builder)
    {
        builder.ToTable("AgentBankings", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
