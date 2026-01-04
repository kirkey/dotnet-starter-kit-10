namespace Accounting.Application.Invoices.LineItems.Update.v1;

/// <summary>
/// Command to update an invoice line item.
/// </summary>
/// <param name>Line item identifier.</param>
/// <param name>Updated description.</param>
/// <param name>Updated quantity.</param>
/// <param name>Updated unit price.</param>
/// <param name>Updated account identifier.</param>
public sealed record UpdateInvoiceLineItemCommand(
    DefaultIdType LineItemId,
    string? Description,
    decimal? Quantity,
    decimal? UnitPrice,
    DefaultIdType? AccountId
) : IRequest<UpdateInvoiceLineItemResponse>;

