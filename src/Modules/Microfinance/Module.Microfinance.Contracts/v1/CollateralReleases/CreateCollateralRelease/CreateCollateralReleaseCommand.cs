using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralReleases.CreateCollateralRelease;

public sealed record CreateCollateralReleaseCommand(string Name) : ICommand<Guid>;
