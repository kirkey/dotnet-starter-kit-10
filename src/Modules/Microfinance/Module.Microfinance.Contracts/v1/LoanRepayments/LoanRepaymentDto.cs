namespace FSH.Module.Microfinance.Contracts.v1.LoanRepayments;

public record LoanRepaymentDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanRepaymentSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
