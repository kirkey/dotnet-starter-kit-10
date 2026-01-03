namespace FSH.Modules.Microfinance.Contracts.v1.LoanRestructures;

public record LoanRestructureDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanRestructureSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
