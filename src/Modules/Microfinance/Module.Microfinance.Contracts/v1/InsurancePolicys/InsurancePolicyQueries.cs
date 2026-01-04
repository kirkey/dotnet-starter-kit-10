namespace FSH.Module.Microfinance.Contracts.v1.InsurancePolicys;

public record GetInsurancePolicyQuery(Guid Id);
public record GetInsurancePolicysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record InsurancePolicysPagedResponse(List<InsurancePolicySummaryDto> Items, int TotalCount, int Page, int PageSize);
