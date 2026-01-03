namespace FSH.Modules.Microfinance.Contracts.v1.CollateralTypes;

public record CollateralTypeDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CollateralTypeSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
