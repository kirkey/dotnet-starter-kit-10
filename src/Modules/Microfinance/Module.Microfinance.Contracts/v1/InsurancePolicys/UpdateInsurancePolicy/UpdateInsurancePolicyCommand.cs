using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsurancePolicys.UpdateInsurancePolicy;

public sealed record UpdateInsurancePolicyCommand(Guid Id, string Name) : ICommand<Guid>;
