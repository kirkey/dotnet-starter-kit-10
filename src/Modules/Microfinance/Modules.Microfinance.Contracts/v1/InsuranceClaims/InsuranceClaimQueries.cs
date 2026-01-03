namespace FSH.Modules.Microfinance.Contracts.v1.InsuranceClaims;

public record GetInsuranceClaimQuery(Guid Id);
public record GetInsuranceClaimsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record InsuranceClaimsPagedResponse(List<InsuranceClaimSummaryDto> Items, int TotalCount, int Page, int PageSize);
