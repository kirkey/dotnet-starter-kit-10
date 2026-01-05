using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.GetInsuranceClaims;

public sealed record GetInsuranceClaimsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InsuranceClaimsPagedResponse>;

public sealed record InsuranceClaimsPagedResponse(List<InsuranceClaimSummaryDto> Items, int TotalCount, int Page, int PageSize);
