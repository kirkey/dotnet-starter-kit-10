using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareProducts.GetShareProducts;

public sealed record GetShareProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<ShareProductsPagedResponse>;

public record ShareProductsPagedResponse(List<ShareProductSummaryDto> Items, int TotalCount, int Page, int PageSize);
