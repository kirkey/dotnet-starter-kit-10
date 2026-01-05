using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralTypes.DeleteCollateralType;

public sealed record DeleteCollateralTypeCommand(Guid Id) : ICommand;
