namespace FSH.Modules.Accounting.Contracts.v1.AccountingPeriods;

public record AccountingPeriodDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record AccountingPeriodSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
