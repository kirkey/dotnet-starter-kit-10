using FSH.Module.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Accounting.Data.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices", "accounting");
        builder.HasKey(x => x.Id);
        
        // Core Properties
        builder.Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(AccountingStringLengths.InvoiceNumber);
        builder.Property(x => x.InvoiceDate).IsRequired();
        builder.Property(x => x.InvoiceType).IsRequired().HasMaxLength(AccountingStringLengths.Small);
        builder.Property(x => x.DueDate).IsRequired();
        
        // Customer/Vendor
        builder.Property(x => x.CustomerId);
        builder.Property(x => x.VendorId);
        builder.Property(x => x.BillToName).IsRequired().HasMaxLength(AccountingStringLengths.Name);
        builder.Property(x => x.BillToAddress).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.ShipToName).HasMaxLength(AccountingStringLengths.Name);
        builder.Property(x => x.ShipToAddress).HasMaxLength(AccountingStringLengths.Description);
        
        // Financial Properties
        builder.Property(x => x.SubTotal).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.TaxAmount).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.DiscountAmount).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.ShippingAmount).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.TotalAmount).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.AmountPaid).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.AmountDue).IsRequired().HasPrecision(18, 2);
        
        // Payment Terms
        builder.Property(x => x.PaymentTerms).HasMaxLength(AccountingStringLengths.Large);
        builder.Property(x => x.TaxCode).HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.CurrencyCode).HasMaxLength(AccountingStringLengths.Small);
        builder.Property(x => x.ExchangeRate).IsRequired().HasPrecision(18, 6);
        
        // Status & Posting
        builder.Property(x => x.Status).IsRequired().HasMaxLength(AccountingStringLengths.Regular);
        builder.Property(x => x.IsPosted).IsRequired();
        builder.Property(x => x.IsPaid).IsRequired();
        builder.Property(x => x.PostedDate);
        builder.Property(x => x.PaidDate);
        builder.Property(x => x.JournalEntryId);
        
        // Reference & Documentation
        builder.Property(x => x.ReferenceNumber).HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.PurchaseOrderNumber).HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.Notes).HasMaxLength(AccountingStringLengths.XHuge);
        builder.Property(x => x.Terms).HasMaxLength(AccountingStringLengths.Large);
        
        // Audit & Status
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);
        
        // Indexes
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.InvoiceNumber }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.InvoiceDate });
        builder.HasIndex(x => new { x.TenantId, x.Status });
        builder.HasIndex(x => x.CustomerId);
        builder.HasIndex(x => x.VendorId);
        builder.HasIndex(x => x.IsPosted);
        builder.HasIndex(x => x.IsPaid);
        builder.HasIndex(x => x.DueDate);
        
        // Ignore navigation properties
        builder.Ignore(x => x.Lines);
    }
}
