namespace FSH.Module.Microfinance.Contracts.v1.CollateralInsurances;

public record CollateralInsuranceDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CollateralInsuranceSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
