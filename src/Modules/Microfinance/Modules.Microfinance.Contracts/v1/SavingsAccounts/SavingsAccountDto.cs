namespace FSH.Modules.Microfinance.Contracts.v1.SavingsAccounts;

public record SavingsAccountDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record SavingsAccountSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
