namespace FSH.Module.Microfinance.Contracts.v1.LoanCollaterals;

public record LoanCollateralDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanCollateralSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
