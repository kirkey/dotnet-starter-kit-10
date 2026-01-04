using FSH.Module.Accounting.Contracts.v1.FixedAssets;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FixedAssets.GetFixedAsset;

/// <summary>
/// Get Fixed Asset query.
/// </summary>
/// <param name="Id">Fixed asset ID to retrieve</param>
public record GetFixedAssetQuery(Guid Id) : IQuery<FixedAssetDto>;
