namespace FSH.Module.Microfinance.Contracts.v1.InvestmentProducts;

public record GetInvestmentProductQuery(Guid Id);
public record GetInvestmentProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record InvestmentProductsPagedResponse(List<InvestmentProductSummaryDto> Items, int TotalCount, int Page, int PageSize);
