namespace FSH.Modules.Microfinance.Contracts.v1.CreditBureauReports;

public record CreditBureauReportDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CreditBureauReportSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
