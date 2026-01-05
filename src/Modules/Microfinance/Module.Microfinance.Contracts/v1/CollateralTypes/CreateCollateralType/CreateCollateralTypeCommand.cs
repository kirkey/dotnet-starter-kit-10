using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralTypes.CreateCollateralType;

public sealed record CreateCollateralTypeCommand(string Name) : ICommand<Guid>;
