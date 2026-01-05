using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsurancePolicys.CreateInsurancePolicy;

public sealed record CreateInsurancePolicyCommand(string Name) : ICommand<Guid>;
