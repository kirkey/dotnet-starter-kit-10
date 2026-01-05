using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FixedAssets.CreateFixedAsset;

/// <summary>
/// Create Fixed Asset command.
/// </summary>
/// <param name="Name">Asset name</param>
/// <param name="Description">Optional description</param>
/// <param name="AcquisitionDate">Optional acquisition date</param>
/// <param name="Cost">Asset cost</param>
/// <param name="ResidualValue">Residual/salvage value</param>
/// <param name="DepreciationRate">Annual depreciation rate (as decimal, e.g., 0.1 for 10%)</param>
public record CreateFixedAssetCommand(
    string Name,
    string? Description = null,
    DateTime? AcquisitionDate = null,
    decimal Cost = 0m,
    decimal ResidualValue = 0m,
    decimal DepreciationRate = 0m) : ICommand<Guid>;