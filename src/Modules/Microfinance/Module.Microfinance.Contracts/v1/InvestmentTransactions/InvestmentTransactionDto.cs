namespace FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions;

public record InvestmentTransactionDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record InvestmentTransactionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
