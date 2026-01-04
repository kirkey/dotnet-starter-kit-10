namespace FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts;

public record InvestmentAccountDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record InvestmentAccountSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
