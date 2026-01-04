namespace FSH.Module.Microfinance.Contracts.v1.MobileTransactions;

public record MobileTransactionDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record MobileTransactionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
