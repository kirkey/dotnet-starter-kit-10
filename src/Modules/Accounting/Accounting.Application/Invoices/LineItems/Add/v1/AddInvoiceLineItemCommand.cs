namespace Accounting.Application.Invoices.LineItems.Add.v1;

/// <summary>
/// Command to add a line item to an invoice.
/// </summary>
/// <param name>Parent invoice identifier.</param>
/// <param name>Line item description.</param>
/// <param name>Quantity of items.</param>
/// <param name>Price per unit.</param>
/// <param name>Optional GL account identifier.</param>
public sealed record AddInvoiceLineItemCommand(
    DefaultIdType InvoiceId,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    DefaultIdType? AccountId
) : IRequest<AddInvoiceLineItemResponse>;

