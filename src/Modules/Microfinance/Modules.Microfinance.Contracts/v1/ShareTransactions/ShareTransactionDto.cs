namespace FSH.Modules.Microfinance.Contracts.v1.ShareTransactions;

public record ShareTransactionDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ShareTransactionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
