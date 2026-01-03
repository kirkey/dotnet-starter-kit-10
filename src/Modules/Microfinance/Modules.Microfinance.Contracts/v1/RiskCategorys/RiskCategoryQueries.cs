namespace FSH.Modules.Microfinance.Contracts.v1.RiskCategorys;

public record GetRiskCategoryQuery(Guid Id);
public record GetRiskCategorysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record RiskCategorysPagedResponse(List<RiskCategorySummaryDto> Items, int TotalCount, int Page, int PageSize);
