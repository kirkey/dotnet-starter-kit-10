namespace FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose;

public record FiscalPeriodCloseDto(
    Guid Id,
    Guid FiscalPeriodId,
    int FiscalYear,
    string PeriodName,
    DateTime StartDate,
    DateTime EndDate,
    DateTime? CloseDate,
    string Status,
    decimal RetainedEarnings,
    Guid? ClosingJournalEntryId,
    Guid? ClosedBy,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record FiscalPeriodCloseSummaryDto(
    Guid Id,
    int FiscalYear,
    string PeriodName,
    DateTime StartDate,
    DateTime EndDate,
    string Status,
    bool IsActive);
