using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanProducts.GetLoanProducts;

public sealed record GetLoanProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanProductsPagedResponse>;
