namespace FSH.Modules.Accounting.Contracts.v1.RecurringJournalEntries;

public record RecurringJournalEntryDto(
    Guid Id,
    string Name,
    string Frequency,
    DateTime? NextRunDate,
    DateTime? LastRunDate,
    Guid? FiscalPeriodId,
    bool IsAutoPost,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record RecurringJournalEntrySummaryDto(
    Guid Id,
    string Name,
    string Frequency,
    DateTime? NextRunDate,
    bool IsActive);
