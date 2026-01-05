using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralInsurances.UpdateCollateralInsurance;

public sealed record UpdateCollateralInsuranceCommand(Guid Id, string Name) : ICommand<Guid>;
