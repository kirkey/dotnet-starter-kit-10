namespace FSH.Module.Microfinance.Contracts.v1.RiskIndicators;

public record RiskIndicatorDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record RiskIndicatorSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
