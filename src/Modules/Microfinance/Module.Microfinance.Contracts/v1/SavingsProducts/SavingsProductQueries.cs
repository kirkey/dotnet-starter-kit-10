namespace FSH.Module.Microfinance.Contracts.v1.SavingsProducts;

public record GetSavingsProductQuery(Guid Id);
public record GetSavingsProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record SavingsProductsPagedResponse(List<SavingsProductSummaryDto> Items, int TotalCount, int Page, int PageSize);
