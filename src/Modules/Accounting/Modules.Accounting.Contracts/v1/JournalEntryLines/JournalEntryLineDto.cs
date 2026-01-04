namespace FSH.Modules.Accounting.Contracts.v1.JournalEntryLines;

public record JournalEntryLineDto(
    Guid Id,
    Guid JournalEntryId,
    int LineNumber,
    Guid AccountId,
    string AccountCode,
    string AccountName,
    decimal Debit,
    decimal Credit,
    decimal Amount,
    string TransactionType,
    string? ReferenceNumber,
    string? Description,
    string? Notes,
    Guid? CostCenterId,
    Guid? DepartmentId,
    Guid? ProjectId,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record JournalEntryLineSummaryDto(
    Guid Id,
    int LineNumber,
    string AccountCode,
    string AccountName,
    decimal Debit,
    decimal Credit,
    bool IsActive);
