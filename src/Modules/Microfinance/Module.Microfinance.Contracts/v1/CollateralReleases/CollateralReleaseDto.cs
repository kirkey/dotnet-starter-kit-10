namespace FSH.Module.Microfinance.Contracts.v1.CollateralReleases;

public record CollateralReleaseDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CollateralReleaseSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
