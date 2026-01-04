namespace FSH.Module.Microfinance.Contracts.v1.LoanApplications;

public record LoanApplicationDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanApplicationSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
