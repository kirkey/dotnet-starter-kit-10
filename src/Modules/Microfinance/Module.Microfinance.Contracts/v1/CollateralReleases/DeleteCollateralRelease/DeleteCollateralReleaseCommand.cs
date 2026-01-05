using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralReleases.DeleteCollateralRelease;

public sealed record DeleteCollateralReleaseCommand(Guid Id) : ICommand;
