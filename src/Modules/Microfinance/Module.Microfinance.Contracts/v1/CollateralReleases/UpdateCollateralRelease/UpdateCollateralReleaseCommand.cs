using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralReleases.UpdateCollateralRelease;

public sealed record UpdateCollateralReleaseCommand(Guid Id, string Name) : ICommand<Guid>;
