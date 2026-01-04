using FSH.Modules.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Accounting.Data.Configurations;

public class ChartOfAccountConfiguration : IEntityTypeConfiguration<ChartOfAccount>
{
    public void Configure(EntityTypeBuilder<ChartOfAccount> builder)
    {
        builder.ToTable("ChartOfAccounts", "accounting");
        builder.HasKey(x => x.Id);
        
        // Account identification
        builder.Property(x => x.AccountCode)
            .IsRequired()
            .HasMaxLength(AccountingStringLengths.AccountCode);
        
        builder.Property(x => x.AccountName)
            .IsRequired()
            .HasMaxLength(AccountingStringLengths.AccountName);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(AccountingStringLengths.AccountName);
        
        builder.Property(x => x.AccountType)
            .IsRequired()
            .HasMaxLength(AccountingStringLengths.AccountType);
        
        // USOA and Hierarchy
        builder.Property(x => x.UsoaCategory)
            .IsRequired()
            .HasMaxLength(AccountingStringLengths.UsoaCategory);
        
        builder.Property(x => x.ParentCode)
            .HasMaxLength(AccountingStringLengths.Regular);
        
        builder.Property(x => x.ParentAccountId);
        
        // Balance and Control
        builder.Property(x => x.Balance)
            .HasPrecision(18, 2);
        
        builder.Property(x => x.NormalBalance)
            .IsRequired()
            .HasMaxLength(AccountingStringLengths.NormalBalance);
        
        builder.Property(x => x.IsControlAccount)
            .IsRequired();
        
        builder.Property(x => x.AllowDirectPosting)
            .IsRequired();
        
        builder.Property(x => x.AccountLevel)
            .IsRequired();
        
        // Compliance and Regulatory
        builder.Property(x => x.IsUsoaCompliant)
            .IsRequired();
        
        builder.Property(x => x.RegulatoryClassification)
            .HasMaxLength(AccountingStringLengths.RegulatoryClassification);
        
        // Status and Metadata
        builder.Property(x => x.IsActive)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasMaxLength(AccountingStringLengths.Description);
        
        builder.Property(x => x.Notes)
            .HasMaxLength(AccountingStringLengths.Notes);
        
        // Multi-tenancy
        builder.Property(x => x.TenantId)
            .IsRequired()
            .HasMaxLength(AccountingStringLengths.TenantId);
        
        // Indexes for performance
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.AccountCode }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.IsActive });
        builder.HasIndex(x => new { x.TenantId, x.AccountType });
        builder.HasIndex(x => new { x.TenantId, x.UsoaCategory });
        builder.HasIndex(x => x.ParentAccountId);
    }
}
