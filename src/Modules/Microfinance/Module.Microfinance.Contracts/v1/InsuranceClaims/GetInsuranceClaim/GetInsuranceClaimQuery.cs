using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.GetInsuranceClaim;

public sealed record GetInsuranceClaimQuery(Guid Id) : IQuery<InsuranceClaimDto>;
