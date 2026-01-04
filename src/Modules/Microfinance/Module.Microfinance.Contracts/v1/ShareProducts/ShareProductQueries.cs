namespace FSH.Module.Microfinance.Contracts.v1.ShareProducts;

public record GetShareProductQuery(Guid Id);
public record GetShareProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record ShareProductsPagedResponse(List<ShareProductSummaryDto> Items, int TotalCount, int Page, int PageSize);
