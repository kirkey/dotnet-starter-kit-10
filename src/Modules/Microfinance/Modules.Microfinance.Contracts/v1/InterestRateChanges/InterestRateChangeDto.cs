namespace FSH.Modules.Microfinance.Contracts.v1.InterestRateChanges;

public record InterestRateChangeDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record InterestRateChangeSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
