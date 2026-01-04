namespace Accounting.Application.DebitMemos.Update;

/// <summary>
/// Command to update a debit memo.
/// </summary>
public sealed record UpdateDebitMemoCommand(
    DefaultIdType Id,
    DateTime? MemoDate,
    decimal? Amount,
    string? Reason,
    string? Description,
    string? Notes
) : IRequest<DefaultIdType>;
