namespace FSH.Module.Microfinance.Contracts.v1.CollateralValuations;

public record CollateralValuationDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CollateralValuationSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
