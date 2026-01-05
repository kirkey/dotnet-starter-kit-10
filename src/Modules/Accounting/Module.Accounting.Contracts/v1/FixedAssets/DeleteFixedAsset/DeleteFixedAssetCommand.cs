using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FixedAssets.DeleteFixedAsset;

/// <summary>
/// Delete Fixed Asset command.
/// </summary>
/// <param name="Id">Fixed asset ID to delete</param>
public record DeleteFixedAssetCommand(Guid Id) : ICommand;