using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsuranceProducts.DeleteInsuranceProduct;

public sealed record DeleteInsuranceProductCommand(Guid Id) : ICommand;
