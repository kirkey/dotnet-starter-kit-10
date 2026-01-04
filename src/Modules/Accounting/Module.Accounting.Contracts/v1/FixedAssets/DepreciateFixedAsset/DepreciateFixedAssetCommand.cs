using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FixedAssets.DepreciateFixedAsset;

/// <summary>
/// Depreciate Fixed Asset command.
/// </summary>
/// <param name="Id">Fixed asset ID to depreciate</param>
/// <param name="Amount">Optional depreciation amount; if null, uses depreciation rate calculation</param>
public record DepreciateFixedAssetCommand(Guid Id, decimal? Amount = null) : ICommand;
