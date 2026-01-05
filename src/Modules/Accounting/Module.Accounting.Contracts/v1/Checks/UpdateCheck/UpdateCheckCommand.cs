using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Checks.UpdateCheck;

/// <summary>
/// Update Check command to update an existing check's payment details and metadata.
/// </summary>
/// <param name="Id">Check ID to update (must exist)</param>
/// <param name="CheckNumber">Updated check number</param>
/// <param name="CheckDate">Updated check date</param>
/// <param name="CheckType">Updated check type</param>
/// <param name="BankAccountId">Updated bank account ID</param>
/// <param name="AccountNumber">Updated account number</param>
/// <param name="Amount">Updated check amount</param>
/// <param name="PayeeName">Updated payee name</param>
/// <param name="PayeeId">Updated payee ID or null</param>
/// <param name="ReferenceNumber">Updated reference number or null</param>
/// <param name="Notes">Updated notes or null</param>
public record UpdateCheckCommand(
    Guid Id,
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