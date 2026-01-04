namespace FSH.Module.Microfinance.Contracts.v1.LoanGuarantors;

public record LoanGuarantorDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanGuarantorSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
