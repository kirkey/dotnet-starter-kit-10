using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskCategorys.GetRiskCategorys;

public sealed record GetRiskCategorysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<RiskCategorysPagedResponse>;

public sealed record RiskCategorysPagedResponse(List<RiskCategorySummaryDto> Items, int TotalCount, int Page, int PageSize);
