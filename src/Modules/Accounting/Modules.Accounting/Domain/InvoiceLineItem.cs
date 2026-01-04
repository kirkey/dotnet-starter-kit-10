namespace FSH.Modules.Accounting.Domain;

/// <summary>
/// Represents an Invoice Line Item in the accounting system.
/// Each line represents a product/service with quantity and pricing.
/// </summary>
public class InvoiceLineItem : AuditableEntity<Guid>, IMustHaveTenant
{
    // Parent Reference
    public Guid InvoiceId { get; private set; }
    
    // Line Identification
    public int LineNumber { get; private set; }
    public string ItemDescription { get; private set; } = default!;
    public string? ItemCode { get; private set; }
    
    // Account Reference
    public Guid AccountId { get; private set; }
    public string AccountCode { get; private set; } = default!;
    
    // Quantity & Pricing
    public decimal Quantity { get; private set; }
    public string? UnitOfMeasure { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal DiscountPercent { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal LineTotal { get; private set; }
    
    // Tax
    public string? TaxCode { get; private set; }
    public decimal TaxRate { get; private set; }
    public decimal TaxAmount { get; private set; }
    
    // Reference & Documentation
    public string? Notes { get; private set; }
    
    // Audit & Status
    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;
    
    // Navigation Properties
    public Invoice? Invoice { get; private set; }
    public ChartOfAccount? Account { get; private set; }
    
    private InvoiceLineItem() { }
    
    public static InvoiceLineItem Create(
        Guid invoiceId,
        int lineNumber,
        string itemDescription,
        Guid accountId,
        string accountCode,
        decimal quantity,
        decimal unitPrice,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? itemCode = null,
        string? unitOfMeasure = null,
        decimal discountPercent = 0,
        string? taxCode = null,
        decimal taxRate = 0,
        string? notes = null)
    {
        // Validation
        if (invoiceId == Guid.Empty)
            throw new BadRequestException("Invoice ID is required");
        if (accountId == Guid.Empty)
            throw new BadRequestException("Account ID is required");
        if (string.IsNullOrWhiteSpace(itemDescription))
            throw new BadRequestException("Item description is required");
        if (string.IsNullOrWhiteSpace(accountCode))
            throw new BadRequestException("Account code is required");
        if (quantity <= 0)
            throw new BadRequestException("Quantity must be positive");
        if (unitPrice < 0)
            throw new BadRequestException("Unit price cannot be negative");
        if (discountPercent < 0 || discountPercent > 100)
            throw new BadRequestException("Discount percent must be between 0 and 100");
        if (taxRate < 0 || taxRate > 100)
            throw new BadRequestException("Tax rate must be between 0 and 100");
        if (lineNumber <= 0)
            throw new BadRequestException("Line number must be positive");
        
        var discountAmount = (quantity * unitPrice) * (discountPercent / 100);
        var lineTotal = (quantity * unitPrice) - discountAmount;
        var taxAmount = lineTotal * (taxRate / 100);
        
        return new InvoiceLineItem
        {
            Id = Guid.NewGuid(),
            InvoiceId = invoiceId,
            LineNumber = lineNumber,
            ItemDescription = itemDescription.Trim(),
            ItemCode = itemCode?.Trim(),
            AccountId = accountId,
            AccountCode = accountCode.Trim(),
            Quantity = quantity,
            UnitOfMeasure = unitOfMeasure?.Trim(),
            UnitPrice = unitPrice,
            DiscountPercent = discountPercent,
            DiscountAmount = discountAmount,
            LineTotal = lineTotal,
            TaxCode = taxCode?.Trim(),
            TaxRate = taxRate,
            TaxAmount = taxAmount,
            Notes = notes?.Trim(),
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }
    
    public void Update(
        int lineNumber,
        string itemDescription,
        Guid accountId,
        string accountCode,
        decimal quantity,
        decimal unitPrice,
        string? itemCode = null,
        string? unitOfMeasure = null,
        decimal discountPercent = 0,
        string? taxCode = null,
        decimal taxRate = 0,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(itemDescription))
            throw new BadRequestException("Item description is required");
        if (quantity <= 0)
            throw new BadRequestException("Quantity must be positive");
        if (unitPrice < 0)
            throw new BadRequestException("Unit price cannot be negative");
        if (discountPercent < 0 || discountPercent > 100)
            throw new BadRequestException("Discount percent must be between 0 and 100");
        if (taxRate < 0 || taxRate > 100)
            throw new BadRequestException("Tax rate must be between 0 and 100");
        
        LineNumber = lineNumber;
        ItemDescription = itemDescription.Trim();
        ItemCode = itemCode?.Trim();
        AccountId = accountId;
        AccountCode = accountCode.Trim();
        Quantity = quantity;
        UnitOfMeasure = unitOfMeasure?.Trim();
        UnitPrice = unitPrice;
        DiscountPercent = discountPercent;
        TaxCode = taxCode?.Trim();
        TaxRate = taxRate;
        Notes = notes?.Trim();
        
        RecalculateAmounts();
    }
    
    public void RecalculateAmounts()
    {
        DiscountAmount = (Quantity * UnitPrice) * (DiscountPercent / 100);
        LineTotal = (Quantity * UnitPrice) - DiscountAmount;
        TaxAmount = LineTotal * (TaxRate / 100);
    }
    
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
