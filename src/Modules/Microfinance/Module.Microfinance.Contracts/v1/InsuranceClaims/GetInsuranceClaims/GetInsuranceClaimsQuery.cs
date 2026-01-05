using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.GetInsuranceClaims;

public sealed record GetInsuranceClaimsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InsuranceClaimsPagedResponse>;
