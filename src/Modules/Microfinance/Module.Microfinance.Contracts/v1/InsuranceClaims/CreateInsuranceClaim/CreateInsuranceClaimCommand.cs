using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.CreateInsuranceClaim;

public sealed record CreateInsuranceClaimCommand(string Name) : ICommand<Guid>;
