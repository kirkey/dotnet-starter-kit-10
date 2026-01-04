namespace FSH.Module.Microfinance.Contracts.v1.InsuranceProducts;

public record GetInsuranceProductQuery(Guid Id);
public record GetInsuranceProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record InsuranceProductsPagedResponse(List<InsuranceProductSummaryDto> Items, int TotalCount, int Page, int PageSize);
