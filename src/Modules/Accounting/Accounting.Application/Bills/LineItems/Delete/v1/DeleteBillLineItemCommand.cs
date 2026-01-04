namespace Accounting.Application.Bills.LineItems.Delete.v1;

/// <summary>
/// Command to delete a bill line item.
/// </summary>
/// <param name="LineItemId">The ID of the line item to delete.</param>
/// <param name="BillId">The ID of the bill (for validation).</param>
public sealed record DeleteBillLineItemCommand(
    DefaultIdType LineItemId,
    DefaultIdType BillId
) : IRequest<DeleteBillLineItemResponse>;
