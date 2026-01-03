namespace FSH.Modules.Microfinance.Contracts.v1.CashVaults;

public record CashVaultDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CashVaultSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
