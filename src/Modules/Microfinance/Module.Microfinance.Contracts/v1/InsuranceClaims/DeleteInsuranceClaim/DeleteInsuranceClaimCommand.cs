using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.DeleteInsuranceClaim;

public sealed record DeleteInsuranceClaimCommand(Guid Id) : ICommand;
