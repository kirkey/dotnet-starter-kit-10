namespace FSH.Modules.Microfinance.Contracts.v1.Loans;

public record LoanDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
