using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsuranceProducts.GetInsuranceProducts;

public sealed record GetInsuranceProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InsuranceProductsPagedResponse>;
