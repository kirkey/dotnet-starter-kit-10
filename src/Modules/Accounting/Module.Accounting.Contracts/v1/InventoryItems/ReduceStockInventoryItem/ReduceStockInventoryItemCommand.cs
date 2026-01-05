using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InventoryItems.ReduceStockInventoryItem;

/// <summary>
/// Reduce Stock of Inventory Item command.
/// </summary>
/// <param name="Id">Inventory item ID</param>
/// <param name="Quantity">Quantity to reduce</param>
public record ReduceStockInventoryItemCommand(Guid Id, decimal Quantity) : ICommand;