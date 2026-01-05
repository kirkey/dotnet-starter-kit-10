using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsurancePolicys.GetInsurancePolicys;

public sealed record GetInsurancePolicysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InsurancePolicysPagedResponse>;
