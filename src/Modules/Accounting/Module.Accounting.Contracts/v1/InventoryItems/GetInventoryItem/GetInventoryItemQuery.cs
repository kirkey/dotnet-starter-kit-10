using FSH.Module.Accounting.Contracts.v1.InventoryItems;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InventoryItems.GetInventoryItem;

/// <summary>
/// Get Inventory Item query.
/// </summary>
/// <param name="Id">Inventory item ID to retrieve</param>
public record GetInventoryItemQuery(Guid Id) : IQuery<InventoryItemDto>;
