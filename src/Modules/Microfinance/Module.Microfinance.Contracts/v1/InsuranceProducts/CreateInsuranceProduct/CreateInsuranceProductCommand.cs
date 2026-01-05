using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsuranceProducts.CreateInsuranceProduct;

public sealed record CreateInsuranceProductCommand(string Name) : ICommand<Guid>;
