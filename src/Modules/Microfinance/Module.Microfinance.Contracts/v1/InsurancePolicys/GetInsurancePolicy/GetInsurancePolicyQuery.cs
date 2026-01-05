using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsurancePolicys.GetInsurancePolicy;

public sealed record GetInsurancePolicyQuery(Guid Id) : IQuery<InsurancePolicyDto>;
