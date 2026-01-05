using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InvoiceLineItems.CreateInvoiceLineItem;

public record CreateInvoiceLineItemCommand(
    Guid InvoiceId,
    int LineNumber,
    string ItemDescription,
    Guid AccountId,
    string AccountCode,
    decimal Quantity,
    decimal UnitPrice,
    string? ItemCode = null,
    string? UnitOfMeasure = null,
    decimal DiscountPercent = 0,
    string? TaxCode = null,
    decimal TaxRate = 0,
    string? Notes = null) : ICommand<Guid>;