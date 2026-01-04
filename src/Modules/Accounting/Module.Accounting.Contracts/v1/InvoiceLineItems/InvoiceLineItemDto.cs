namespace FSH.Module.Accounting.Contracts.v1.InvoiceLineItems;

public record InvoiceLineItemDto(
    Guid Id,
    Guid InvoiceId,
    int LineNumber,
    string ItemDescription,
    string? ItemCode,
    Guid AccountId,
    string AccountCode,
    decimal Quantity,
    string? UnitOfMeasure,
    decimal UnitPrice,
    decimal DiscountPercent,
    decimal DiscountAmount,
    decimal LineTotal,
    string? TaxCode,
    decimal TaxRate,
    decimal TaxAmount,
    string? Notes,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record InvoiceLineItemSummaryDto(
    Guid Id,
    int LineNumber,
    string ItemDescription,
    string AccountCode,
    decimal Quantity,
    decimal UnitPrice,
    decimal LineTotal,
    bool IsActive);
