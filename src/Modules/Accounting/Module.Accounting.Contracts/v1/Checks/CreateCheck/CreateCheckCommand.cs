using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Checks.CreateCheck;

/// <summary>
/// Create Check command to create a new check payment with bank account and payee details.
/// </summary>
/// <param name="CheckNumber">Check number (required, unique per bank account)</param>
/// <param name="CheckDate">Check date/issuance date (required)</param>
/// <param name="CheckType">Check type classifier (e.g., "Personal", "Corporate", "Payroll")</param>
/// <param name="BankAccountId">Bank account ID from which check is drawn (required)</param>
/// <param name="AccountNumber">Bank account number (required for reconciliation)</param>
/// <param name="Amount">Check amount in bank account currency (required, positive)</param>
/// <param name="PayeeName">Payee name or company name (required)</param>
/// <param name="PayeeId">Optional reference to Customer/Vendor/Employee entity</param>
/// <param name="ReferenceNumber">Optional invoice/bill reference number for matching</param>
/// <param name="Notes">Optional memo or payment notes for recipient</param>
public record CreateCheckCommand(
    string CheckNumber,
    DateTime CheckDate,
    string CheckType,
    Guid BankAccountId,
    string AccountNumber,
    decimal Amount,
    string PayeeName,
    Guid? PayeeId = null,
    string? ReferenceNumber = null,
    string? Notes = null) : ICommand<Guid>;
