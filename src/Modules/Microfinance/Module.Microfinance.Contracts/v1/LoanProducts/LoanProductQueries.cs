namespace FSH.Module.Microfinance.Contracts.v1.LoanProducts;

public record GetLoanProductQuery(Guid Id);
public record GetLoanProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoanProductsPagedResponse(List<LoanProductSummaryDto> Items, int TotalCount, int Page, int PageSize);
