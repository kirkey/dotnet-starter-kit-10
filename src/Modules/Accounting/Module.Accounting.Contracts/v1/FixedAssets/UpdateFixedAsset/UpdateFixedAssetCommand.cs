using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FixedAssets.UpdateFixedAsset;

/// <summary>
/// Update Fixed Asset command.
/// </summary>
/// <param name="Id">Fixed asset ID to update</param>
/// <param name="Name">Updated name</param>
/// <param name="Description">Updated description or null</param>
/// <param name="AcquisitionDate">Updated acquisition date or null</param>
/// <param name="Cost">Updated cost or null</param>
/// <param name="ResidualValue">Updated residual value or null</param>
/// <param name="DepreciationRate">Updated depreciation rate or null</param>
public record UpdateFixedAssetCommand(
    Guid Id,
    string Name,
    string? Description = null,
    DateTime? AcquisitionDate = null,
    decimal? Cost = null,
    decimal? ResidualValue = null,
    decimal? DepreciationRate = null) : ICommand<Guid>;
