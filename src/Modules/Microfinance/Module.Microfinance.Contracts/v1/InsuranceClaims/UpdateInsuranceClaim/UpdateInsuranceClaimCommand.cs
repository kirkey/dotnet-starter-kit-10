using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.UpdateInsuranceClaim;

public sealed record UpdateInsuranceClaimCommand(Guid Id, string Name) : ICommand<Guid>;
