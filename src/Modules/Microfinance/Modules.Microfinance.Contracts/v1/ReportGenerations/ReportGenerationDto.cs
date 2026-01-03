namespace FSH.Modules.Microfinance.Contracts.v1.ReportGenerations;

public record ReportGenerationDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ReportGenerationSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
