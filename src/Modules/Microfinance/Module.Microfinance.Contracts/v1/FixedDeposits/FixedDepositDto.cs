namespace FSH.Module.Microfinance.Contracts.v1.FixedDeposits;

public record FixedDepositDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record FixedDepositSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
