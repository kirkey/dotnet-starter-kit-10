namespace Accounting.Application.CreditMemos.Create;

/// <summary>
/// Command to create a new credit memo.
/// </summary>
public sealed record CreateCreditMemoCommand(
    string MemoNumber,
    DateTime MemoDate,
    decimal Amount,
    string ReferenceType,
    DefaultIdType ReferenceId,
    DefaultIdType? OriginalDocumentId,
    string? Reason,
    string? Description,
    string? Notes
) : IRequest<DefaultIdType>;
