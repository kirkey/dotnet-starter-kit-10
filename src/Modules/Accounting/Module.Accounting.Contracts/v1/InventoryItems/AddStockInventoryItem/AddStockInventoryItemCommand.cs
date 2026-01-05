using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InventoryItems.AddStockInventoryItem;

/// <summary>
/// Add Stock to Inventory Item command.
/// </summary>
/// <param name="Id">Inventory item ID</param>
/// <param name="Quantity">Quantity to add</param>
public record AddStockInventoryItemCommand(Guid Id, decimal Quantity) : ICommand;