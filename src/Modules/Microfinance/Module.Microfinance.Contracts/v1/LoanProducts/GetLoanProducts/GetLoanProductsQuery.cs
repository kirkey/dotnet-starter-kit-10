using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanProducts.GetLoanProducts;

public sealed record GetLoanProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanProductsPagedResponse>;

public sealed record LoanProductsPagedResponse(List<LoanProductSummaryDto> Items, int TotalCount, int Page, int PageSize);
