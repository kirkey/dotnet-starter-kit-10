namespace Accounting.Application.CreditMemos.Update;

/// <summary>
/// Command to update an existing credit memo.
/// </summary>
public sealed record UpdateCreditMemoCommand(
    DefaultIdType Id,
    string? Description,
    string? Notes,
    string? Reason
) : IRequest<DefaultIdType>;
