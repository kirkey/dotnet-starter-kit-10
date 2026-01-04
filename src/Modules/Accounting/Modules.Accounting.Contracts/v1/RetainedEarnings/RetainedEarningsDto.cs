namespace FSH.Modules.Accounting.Contracts.v1.RetainedEarnings;

public record RetainedEarningsDto(
    Guid Id,
    string Name,
    string? Description,
    int? FiscalYear,
    decimal OpeningBalance,
    decimal ClosingBalance,
    bool IsClosed,
    DateTime? ClosedOn,
    Guid? ClosedBy,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record RetainedEarningsSummaryDto(
    Guid Id,
    string Name,
    int? FiscalYear,
    decimal ClosingBalance,
    bool IsClosed,
    bool IsActive);
