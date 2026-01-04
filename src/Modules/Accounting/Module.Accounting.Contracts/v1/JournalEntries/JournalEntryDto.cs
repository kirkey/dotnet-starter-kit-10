namespace FSH.Module.Accounting.Contracts.v1.JournalEntries;

public record JournalEntryDto(
    Guid Id,
    string EntryNumber,
    DateTime EntryDate,
    string EntryType,
    string ReferenceNumber,
    string? ReferenceType,
    decimal TotalDebit,
    decimal TotalCredit,
    Guid FiscalPeriodId,
    string Status,
    DateTime? PostedDate,
    Guid? PostedBy,
    DateTime? ApprovedDate,
    Guid? ApprovedBy,
    bool IsReversed,
    Guid? ReversedEntryId,
    DateTime? ReversedDate,
    string? Description,
    string? Notes,
    string? Memo,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record JournalEntrySummaryDto(
    Guid Id,
    string EntryNumber,
    DateTime EntryDate,
    string EntryType,
    string ReferenceNumber,
    decimal TotalDebit,
    decimal TotalCredit,
    string Status,
    bool IsActive);
