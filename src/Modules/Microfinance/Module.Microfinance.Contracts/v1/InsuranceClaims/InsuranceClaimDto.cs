namespace FSH.Module.Microfinance.Contracts.v1.InsuranceClaims;

public record InsuranceClaimDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record InsuranceClaimSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
