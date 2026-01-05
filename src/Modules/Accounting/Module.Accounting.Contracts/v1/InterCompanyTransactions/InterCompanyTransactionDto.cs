namespace FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions;

public record InterCompanyTransactionDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record InterCompanyTransactionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);