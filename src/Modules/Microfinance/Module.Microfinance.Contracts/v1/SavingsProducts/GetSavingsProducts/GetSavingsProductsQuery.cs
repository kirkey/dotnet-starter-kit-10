using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsProducts.GetSavingsProducts;

public sealed record GetSavingsProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<SavingsProductsPagedResponse>;

public sealed record SavingsProductsPagedResponse(List<SavingsProductSummaryDto> Items, int TotalCount, int Page, int PageSize);
