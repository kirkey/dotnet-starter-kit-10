using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentProducts.GetInvestmentProducts;

public sealed record GetInvestmentProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InvestmentProductsPagedResponse>;

public sealed record InvestmentProductsPagedResponse(List<InvestmentProductSummaryDto> Items, int TotalCount, int Page, int PageSize);
