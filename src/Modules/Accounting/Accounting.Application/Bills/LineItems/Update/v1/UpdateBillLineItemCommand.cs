namespace Accounting.Application.Bills.LineItems.Update.v1;

/// <summary>
/// Command to update an existing bill line item.
/// </summary>
/// <param name>The ID of the line item to update.</param>
/// <param name>The ID of the bill (for validation).</param>
/// <param name>Updated line number for ordering.</param>
/// <param name>Updated description of goods/services.</param>
/// <param name>Updated quantity of items.</param>
/// <param name>Updated price per unit.</param>
/// <param name>Updated extended line amount.</param>
/// <param name>Updated GL account for posting.</param>
/// <param name>Optional updated tax code.</param>
/// <param name>Updated tax amount.</param>
/// <param name>Optional updated project reference.</param>
/// <param name>Optional updated cost center reference.</param>
/// <param name>Optional updated notes.</param>
public sealed record UpdateBillLineItemCommand(
    DefaultIdType LineItemId,
    DefaultIdType BillId,
    int LineNumber,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal Amount,
    DefaultIdType ChartOfAccountId,
    DefaultIdType? TaxCodeId,
    decimal TaxAmount,
    DefaultIdType? ProjectId,
    DefaultIdType? CostCenterId,
    string? Notes
) : IRequest<UpdateBillLineItemResponse>;

