namespace FSH.Module.Microfinance.Contracts.v1.FeeCharges;

public record FeeChargeDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record FeeChargeSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
