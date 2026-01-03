namespace FSH.Modules.Microfinance.Contracts.v1.LoanDisbursementTranches;

public record LoanDisbursementTrancheDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanDisbursementTrancheSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
