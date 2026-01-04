using FSH.Modules.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Accounting.Data.Configurations;

public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
{
    public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
    {
        builder.ToTable("InvoiceLineItems", "accounting");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.InvoiceId).IsRequired();
        builder.Property(x => x.LineNumber).IsRequired();
        builder.Property(x => x.ItemDescription).IsRequired().HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.ItemCode).HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.AccountId).IsRequired();
        builder.Property(x => x.AccountCode).IsRequired().HasMaxLength(AccountingStringLengths.AccountCode);
        builder.Property(x => x.Quantity).IsRequired().HasPrecision(18, 4);
        builder.Property(x => x.UnitOfMeasure).HasMaxLength(AccountingStringLengths.Small);
        builder.Property(x => x.UnitPrice).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.DiscountPercent).IsRequired().HasPrecision(5, 2);
        builder.Property(x => x.DiscountAmount).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.LineTotal).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.TaxCode).HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.TaxRate).IsRequired().HasPrecision(5, 2);
        builder.Property(x => x.TaxAmount).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.Notes).HasMaxLength(AccountingStringLengths.XHuge);
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);
        
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.InvoiceId });
        builder.HasIndex(x => new { x.InvoiceId, x.LineNumber }).IsUnique();
        builder.HasIndex(x => x.AccountId);
        
        builder.Ignore(x => x.Invoice);
        builder.Ignore(x => x.Account);
    }
}
