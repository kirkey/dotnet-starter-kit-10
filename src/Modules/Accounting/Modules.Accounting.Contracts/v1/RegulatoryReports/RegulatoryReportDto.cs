namespace FSH.Modules.Accounting.Contracts.v1.RegulatoryReports;

public record RegulatoryReportDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record RegulatoryReportSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
