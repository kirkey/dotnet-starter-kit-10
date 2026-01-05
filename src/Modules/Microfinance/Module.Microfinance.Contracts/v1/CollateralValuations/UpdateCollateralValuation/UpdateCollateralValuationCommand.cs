using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralValuations.UpdateCollateralValuation;

public sealed record UpdateCollateralValuationCommand(Guid Id, string Name) : ICommand<Guid>;
