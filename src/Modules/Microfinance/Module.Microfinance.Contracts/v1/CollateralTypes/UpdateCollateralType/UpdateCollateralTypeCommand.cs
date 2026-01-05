using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralTypes.UpdateCollateralType;

public sealed record UpdateCollateralTypeCommand(Guid Id, string Name) : ICommand<Guid>;
