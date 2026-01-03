namespace FSH.Modules.Microfinance.Contracts.v1.RiskAlerts;

public record RiskAlertDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record RiskAlertSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
