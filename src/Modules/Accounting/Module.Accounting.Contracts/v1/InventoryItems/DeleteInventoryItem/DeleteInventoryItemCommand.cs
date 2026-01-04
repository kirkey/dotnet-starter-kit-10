using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InventoryItems.DeleteInventoryItem;

/// <summary>
/// Delete Inventory Item command.
/// </summary>
/// <param name="Id">Inventory item ID to delete</param>
public record DeleteInventoryItemCommand(Guid Id) : ICommand;
