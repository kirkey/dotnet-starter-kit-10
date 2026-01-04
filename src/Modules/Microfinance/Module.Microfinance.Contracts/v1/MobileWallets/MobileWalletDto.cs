namespace FSH.Module.Microfinance.Contracts.v1.MobileWallets;

public record MobileWalletDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record MobileWalletSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
