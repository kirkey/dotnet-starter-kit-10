using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralValuations.DeleteCollateralValuation;

public sealed record DeleteCollateralValuationCommand(Guid Id) : ICommand;
