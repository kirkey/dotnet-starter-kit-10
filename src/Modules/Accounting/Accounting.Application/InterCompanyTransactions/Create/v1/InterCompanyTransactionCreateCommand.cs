namespace Accounting.Application.InterCompanyTransactions.Create.v1;

/// <summary>
/// Command to create a new inter-company transaction.
/// </summary>
public record InterCompanyTransactionCreateCommand(
    string TransactionNumber,
    DefaultIdType FromEntityId,
    string FromEntityName,
    DefaultIdType ToEntityId,
    string ToEntityName,
    DateTime TransactionDate,
    decimal Amount,
    string TransactionType,
    DefaultIdType FromAccountId,
    DefaultIdType ToAccountId,
    string? ReferenceNumber,
    DateTime? DueDate,
    bool RequiresElimination,
    DefaultIdType? PeriodId,
    string? Description,
    string? Notes
) : IRequest<InterCompanyTransactionCreateResponse>;

