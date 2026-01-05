using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralInsurances.DeleteCollateralInsurance;

public sealed record DeleteCollateralInsuranceCommand(Guid Id) : ICommand;
