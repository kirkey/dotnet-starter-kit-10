using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InventoryItems.GetListInventoryItem;

/// <summary>
/// Get Inventory Items (paginated) query.
/// </summary>
/// <param name="Page">Page number for pagination (1-based, default=1)</param>
/// <param name="PageSize">Number of items per page (default=10)</param>
/// <param name="SearchTerm">Optional filter by name (contains search)</param>
/// <param name="IsActive">Optional filter by active status</param>
public record GetInventoryItemsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<InventoryItemsPagedResponse>;

/// <summary>
/// Response object for paginated inventory item list.
/// </summary>
/// <param name="Items">List of InventoryItemSummaryDto</param>
/// <param name="TotalCount">Total count of items matching filters</param>
/// <param name="Page">Requested page number</param>
/// <param name="PageSize">Items per page</param>
public record InventoryItemsPagedResponse(
    List<InventoryItemSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Summary DTO for inventory item list responses.
/// </summary>
/// <param name="Id">Inventory item ID</param>
/// <param name="Name">Item name</param>
/// <param name="IsActive">Active flag</param>
public record InventoryItemSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);