using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralInsurances.CreateCollateralInsurance;

public sealed record CreateCollateralInsuranceCommand(string Name) : ICommand<Guid>;
