namespace FSH.Module.Microfinance.Contracts.v1.InsurancePolicys;

public record InsurancePolicyDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record InsurancePolicySummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
