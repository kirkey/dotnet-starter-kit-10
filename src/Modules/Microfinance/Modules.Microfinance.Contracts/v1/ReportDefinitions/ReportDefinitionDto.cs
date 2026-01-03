namespace FSH.Modules.Microfinance.Contracts.v1.ReportDefinitions;

public record ReportDefinitionDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ReportDefinitionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
