using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FixedAssets.GetListFixedAsset;

/// <summary>
/// Get Fixed Assets (paginated) query.
/// </summary>
/// <param name="Page">Page number for pagination (1-based, default=1)</param>
/// <param name="PageSize">Number of items per page (default=10)</param>
/// <param name="SearchTerm">Optional filter by name (contains search)</param>
/// <param name="IsActive">Optional filter by active status</param>
public record GetFixedAssetsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<FixedAssetsPagedResponse>;

/// <summary>
/// Response object for paginated fixed asset list.
/// </summary>
/// <param name="Items">List of FixedAssetSummaryDto</param>
/// <param name="TotalCount">Total count of assets matching filters</param>
/// <param name="Page">Requested page number</param>
/// <param name="PageSize">Items per page</param>
public record FixedAssetsPagedResponse(
    List<FixedAssetSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Summary DTO for fixed asset list responses.
/// </summary>
/// <param name="Id">Fixed asset ID</param>
/// <param name="Name">Asset name</param>
/// <param name="Cost">Asset cost</param>
/// <param name="AccumulatedDepreciation">Total accumulated depreciation</param>
/// <param name="IsDisposed">Whether asset has been disposed</param>
/// <param name="IsActive">Active flag</param>
public record FixedAssetSummaryDto(
    Guid Id,
    string Name,
    decimal Cost,
    decimal AccumulatedDepreciation,
    bool IsDisposed,
    bool IsActive);
