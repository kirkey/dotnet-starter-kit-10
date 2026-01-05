using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsurancePolicys.DeleteInsurancePolicy;

public sealed record DeleteInsurancePolicyCommand(Guid Id) : ICommand;
