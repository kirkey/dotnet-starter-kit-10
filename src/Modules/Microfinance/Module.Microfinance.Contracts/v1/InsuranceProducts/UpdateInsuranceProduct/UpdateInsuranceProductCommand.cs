using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsuranceProducts.UpdateInsuranceProduct;

public sealed record UpdateInsuranceProductCommand(Guid Id, string Name) : ICommand<Guid>;
