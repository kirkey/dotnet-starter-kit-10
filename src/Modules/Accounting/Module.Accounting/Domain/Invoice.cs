namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents an Invoice (AR or AP) in the accounting system.
/// Supports both Accounts Receivable (customer invoices) and Accounts Payable (vendor bills).
/// </summary>
public class Invoice : AuditableEntity<Guid>, IMustHaveTenant
{
    // Core Properties
    public string InvoiceNumber { get; private set; } = default!;
    public DateTime InvoiceDate { get; private set; }
    public string InvoiceType { get; private set; } = default!; // "AR" (Receivable) or "AP" (Payable)
    public DateTime DueDate { get; private set; }
    
    // Customer/Vendor Information
    public Guid? CustomerId { get; private set; }
    public Guid? VendorId { get; private set; }
    public string BillToName { get; private set; } = default!;
    public string? BillToAddress { get; private set; }
    public string? ShipToName { get; private set; }
    public string? ShipToAddress { get; private set; }
    
    // Financial Properties
    public decimal SubTotal { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal ShippingAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal AmountPaid { get; private set; }
    public decimal AmountDue { get; private set; }
    
    // Payment Terms
    public string? PaymentTerms { get; private set; }
    public string? TaxCode { get; private set; }
    public string? CurrencyCode { get; private set; }
    public decimal ExchangeRate { get; private set; } = 1.0m;
    
    // Status & Posting
    public string Status { get; private set; } = "Draft"; // Draft, Sent, Approved, Paid, Void, Overdue
    public bool IsPosted { get; private set; }
    public bool IsPaid { get; private set; }
    public DateTime? PostedDate { get; private set; }
    public DateTime? PaidDate { get; private set; }
    
    // Related Journal Entry
    public Guid? JournalEntryId { get; private set; }
    
    // Reference & Documentation
    public string? ReferenceNumber { get; private set; }
    public string? PurchaseOrderNumber { get; private set; }
    public string? Description { get; private set; }
    public string? Notes { get; private set; }
    public string? Terms { get; private set; }
    
    // Audit & Status
    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;
    
    // Navigation Properties
    public ICollection<InvoiceLineItem>? Lines { get; private set; }
    
    private Invoice() { }
    
    public static Invoice Create(
        string invoiceNumber,
        DateTime invoiceDate,
        string invoiceType,
        DateTime dueDate,
        string billToName,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        Guid? customerId = null,
        Guid? vendorId = null,
        string? billToAddress = null,
        string? shipToName = null,
        string? shipToAddress = null,
        string? paymentTerms = null,
        string? taxCode = null,
        string? currencyCode = null,
        decimal exchangeRate = 1.0m,
        string? referenceNumber = null,
        string? purchaseOrderNumber = null,
        string? description = null,
        string? notes = null,
        string? terms = null)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(invoiceNumber))
            throw new BadRequestException("Invoice number is required");
        if (string.IsNullOrWhiteSpace(billToName))
            throw new BadRequestException("Bill to name is required");
        if (!IsValidInvoiceType(invoiceType))
            throw new BadRequestException("Invoice type must be AR or AP");
        if (invoiceType == "AR" && !customerId.HasValue)
            throw new BadRequestException("Customer is required for AR invoices");
        if (invoiceType == "AP" && !vendorId.HasValue)
            throw new BadRequestException("Vendor is required for AP invoices");
        if (exchangeRate <= 0)
            throw new BadRequestException("Exchange rate must be positive");
        
        return new Invoice
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = invoiceNumber.Trim(),
            InvoiceDate = invoiceDate.Date,
            InvoiceType = invoiceType.Trim().ToUpper(),
            DueDate = dueDate.Date,
            CustomerId = customerId,
            VendorId = vendorId,
            BillToName = billToName.Trim(),
            BillToAddress = billToAddress?.Trim(),
            ShipToName = shipToName?.Trim(),
            ShipToAddress = shipToAddress?.Trim(),
            SubTotal = 0,
            TaxAmount = 0,
            DiscountAmount = 0,
            ShippingAmount = 0,
            TotalAmount = 0,
            AmountPaid = 0,
            AmountDue = 0,
            PaymentTerms = paymentTerms?.Trim(),
            TaxCode = taxCode?.Trim(),
            CurrencyCode = currencyCode?.Trim() ?? "USD",
            ExchangeRate = exchangeRate,
            Status = "Draft",
            IsPosted = false,
            IsPaid = false,
            ReferenceNumber = referenceNumber?.Trim(),
            PurchaseOrderNumber = purchaseOrderNumber?.Trim(),
            Description = description?.Trim(),
            Notes = notes?.Trim(),
            Terms = terms?.Trim(),
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }
    
    public void Update(
        string invoiceNumber,
        DateTime invoiceDate,
        DateTime dueDate,
        string billToName,
        Guid? customerId = null,
        Guid? vendorId = null,
        string? billToAddress = null,
        string? shipToName = null,
        string? shipToAddress = null,
        string? paymentTerms = null,
        string? taxCode = null,
        string? currencyCode = null,
        decimal exchangeRate = 1.0m,
        string? referenceNumber = null,
        string? purchaseOrderNumber = null,
        string? description = null,
        string? notes = null,
        string? terms = null)
    {
        if (IsPosted)
            throw new BadRequestException("Cannot update a posted invoice");
        if (string.IsNullOrWhiteSpace(billToName))
            throw new BadRequestException("Bill to name is required");
        if (exchangeRate <= 0)
            throw new BadRequestException("Exchange rate must be positive");
        
        InvoiceNumber = invoiceNumber.Trim();
        InvoiceDate = invoiceDate.Date;
        DueDate = dueDate.Date;
        CustomerId = customerId;
        VendorId = vendorId;
        BillToName = billToName.Trim();
        BillToAddress = billToAddress?.Trim();
        ShipToName = shipToName?.Trim();
        ShipToAddress = shipToAddress?.Trim();
        PaymentTerms = paymentTerms?.Trim();
        TaxCode = taxCode?.Trim();
        CurrencyCode = currencyCode?.Trim() ?? "USD";
        ExchangeRate = exchangeRate;
        ReferenceNumber = referenceNumber?.Trim();
        PurchaseOrderNumber = purchaseOrderNumber?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();
        Terms = terms?.Trim();
    }
    
    public void CalculateTotals(decimal subTotal, decimal taxAmount, decimal discountAmount, decimal shippingAmount)
    {
        SubTotal = subTotal;
        TaxAmount = taxAmount;
        DiscountAmount = discountAmount;
        ShippingAmount = shippingAmount;
        TotalAmount = SubTotal + TaxAmount - DiscountAmount + ShippingAmount;
        AmountDue = TotalAmount - AmountPaid;
    }
    
    public void Post(Guid journalEntryId, DateTime? postedDate = null)
    {
        if (IsPosted)
            throw new BadRequestException("Invoice is already posted");
        if (TotalAmount <= 0)
            throw new BadRequestException("Invoice total must be greater than zero");
        
        IsPosted = true;
        JournalEntryId = journalEntryId;
        PostedDate = postedDate ?? DateTime.UtcNow;
        Status = "Sent";
    }
    
    public void MarkAsPaid(decimal paymentAmount, DateTime? paidDate = null)
    {
        if (!IsPosted)
            throw new BadRequestException("Invoice must be posted before marking as paid");
        
        AmountPaid += paymentAmount;
        AmountDue = TotalAmount - AmountPaid;
        
        if (AmountDue <= 0)
        {
            IsPaid = true;
            PaidDate = paidDate ?? DateTime.UtcNow;
            Status = "Paid";
        }
        else
        {
            Status = "Partial";
        }
    }
    
    public void Void()
    {
        if (IsPaid)
            throw new BadRequestException("Cannot void a paid invoice. Create a credit note instead");
        
        Status = "Void";
        IsActive = false;
    }
    
    public void Approve()
    {
        if (IsPosted)
            throw new BadRequestException("Invoice is already posted");
        
        Status = "Approved";
    }
    
    public void MarkAsOverdue()
    {
        if (!IsPaid && DateTime.UtcNow.Date > DueDate.Date)
        {
            Status = "Overdue";
        }
    }
    
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    
    private static bool IsValidInvoiceType(string invoiceType)
    {
        return invoiceType.Equals("AR", StringComparison.OrdinalIgnoreCase) ||
               invoiceType.Equals("AP", StringComparison.OrdinalIgnoreCase);
    }
}
