using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FixedAssets.DisposeFixedAsset;

/// <summary>
/// Dispose Fixed Asset command - removes asset from active use.
/// </summary>
/// <param name="Id">Fixed asset ID to dispose</param>
/// <param name="DisposalDate">Optional disposal date; defaults to current date</param>
/// <param name="Proceeds">Optional proceeds from asset disposal</param>
public record DisposeFixedAssetCommand(Guid Id, DateTime? DisposalDate = null, decimal? Proceeds = null) : ICommand;
