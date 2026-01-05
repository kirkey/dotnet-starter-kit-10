using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralValuations.CreateCollateralValuation;

public sealed record CreateCollateralValuationCommand(string Name) : ICommand<Guid>;
