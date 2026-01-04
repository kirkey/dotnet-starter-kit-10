namespace Accounting.Application.AccountsReceivableAccounts.Create.v1;

/// <summary>
/// Command to create a new accounts receivable account.
/// </summary>
public record AccountsReceivableAccountCreateCommand(
    string AccountNumber,
    string AccountName,
    DefaultIdType? GeneralLedgerAccountId,
    DefaultIdType? PeriodId,
    string? Description,
    string? Notes
) : IRequest<AccountsReceivableAccountCreateResponse>;

