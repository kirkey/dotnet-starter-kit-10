namespace Accounting.Application.Bills.Create.v1;

/// <summary>
/// Command to create a new bill.
/// </summary>
/// <param name>Unique bill or vendor invoice number.</param>
/// <param name>Vendor who issued the bill.</param>
/// <param name>Date bill was issued.</param>
/// <param name>Date payment is due.</param>
/// <param name>Description of the bill.</param>
/// <param name>Optional accounting period.</param>
/// <param name>Optional payment terms.</param>
/// <param name>Optional PO reference.</param>
/// <param name>Optional notes.</param>
/// <param name>Line items for the bill.</param>
public sealed record CreateBillCommand(
    string BillNumber,
    DefaultIdType VendorId,
    DateTime BillDate,
    DateTime DueDate,
    string? Description,
    DefaultIdType? PeriodId,
    string? PaymentTerms,
    string? PurchaseOrderNumber,
    string? Notes,
    List<BillLineItemDto>? LineItems = null
) : IRequest<BillCreateResponse>;

/// <summary>
/// DTO for bill line items in create/update commands.
/// </summary>
/// <param name>Line number for ordering.</param>
/// <param name>Description of goods/services.</param>
/// <param name>Quantity of items.</param>
/// <param name>Price per unit.</param>
/// <param name>Extended line amount.</param>
/// <param name>GL account for posting.</param>
/// <param name>Optional tax code.</param>
/// <param name>Optional tax amount.</param>
/// <param name>Optional project reference.</param>
/// <param name>Optional cost center reference.</param>
/// <param name>Optional notes.</param>
public sealed record BillLineItemDto(
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
);

