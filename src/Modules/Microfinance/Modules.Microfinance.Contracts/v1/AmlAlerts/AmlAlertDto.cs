namespace FSH.Modules.Microfinance.Contracts.v1.AmlAlerts;

public record AmlAlertDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record AmlAlertSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
