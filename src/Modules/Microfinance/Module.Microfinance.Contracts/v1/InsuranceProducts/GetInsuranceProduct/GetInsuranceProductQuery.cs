using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsuranceProducts.GetInsuranceProduct;

public sealed record GetInsuranceProductQuery(Guid Id) : IQuery<InsuranceProductDto>;
