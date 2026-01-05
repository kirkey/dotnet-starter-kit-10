using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsurancePolicys.GetInsurancePolicys;

public sealed record GetInsurancePolicysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InsurancePolicysPagedResponse>;

public sealed record InsurancePolicysPagedResponse(List<InsurancePolicySummaryDto> Items, int TotalCount, int Page, int PageSize);
