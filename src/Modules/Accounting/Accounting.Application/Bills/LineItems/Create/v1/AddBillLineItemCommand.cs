namespace Accounting.Application.Bills.LineItems.Create.v1;

/// <summary>
/// Command to add a new line item to a bill.
/// </summary>
/// <param name>The ID of the bill to add the line item to.</param>
/// <param name>Line number for ordering (must be positive).</param>
/// <param name>Description of goods/services (required, max 500 chars).</param>
/// <param name>Quantity of items (must be greater than zero).</param>
/// <param name>Price per unit (cannot be negative).</param>
/// <param name>Extended line amount (should equal Quantity × UnitPrice).</param>
/// <param name>GL account for posting (required).</param>
/// <param name>Optional tax code.</param>
/// <param name>Tax amount (cannot be negative, default 0).</param>
/// <param name>Optional project reference.</param>
/// <param name>Optional cost center reference.</param>
/// <param name>Optional notes (max 1000 chars).</param>
public sealed record AddBillLineItemCommand(
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
) : IRequest<AddBillLineItemResponse>;
