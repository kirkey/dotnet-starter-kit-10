namespace Accounting.Application.Bills.Delete.v1;

/// <summary>
/// Command to delete a bill.
/// </summary>
/// <param name="BillId">The ID of the bill to delete.</param>
public sealed record DeleteBillCommand(
    DefaultIdType BillId
) : IRequest<DeleteBillResponse>;
