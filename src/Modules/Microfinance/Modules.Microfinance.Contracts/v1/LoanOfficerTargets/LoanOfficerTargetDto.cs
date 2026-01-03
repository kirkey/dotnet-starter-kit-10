namespace FSH.Modules.Microfinance.Contracts.v1.LoanOfficerTargets;

public record LoanOfficerTargetDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanOfficerTargetSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
