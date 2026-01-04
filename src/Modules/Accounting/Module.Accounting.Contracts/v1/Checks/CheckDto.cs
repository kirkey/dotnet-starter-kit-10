namespace FSH.Module.Accounting.Contracts.v1.Checks;

public record CheckDto(
    Guid Id,
    string CheckNumber,
    DateTime CheckDate,
    string CheckType,
    Guid? PayeeId,
    string PayeeName,
    Guid BankAccountId,
    string AccountNumber,
    decimal Amount,
    string Status,
    DateTime? PrintedDate,
    DateTime? ClearedDate,
    Guid? ClearedBy,
    Guid? JournalEntryId,
    string? ReferenceNumber,
    string? Notes,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CheckSummaryDto(
    Guid Id,
    string CheckNumber,
    DateTime CheckDate,
    string PayeeName,
    decimal Amount,
    string Status,
    bool IsActive);
