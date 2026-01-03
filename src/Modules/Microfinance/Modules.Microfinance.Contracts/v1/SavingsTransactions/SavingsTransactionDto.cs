namespace FSH.Modules.Microfinance.Contracts.v1.SavingsTransactions;

public record SavingsTransactionDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record SavingsTransactionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
