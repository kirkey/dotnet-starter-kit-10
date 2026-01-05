using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InventoryItems.UpdateInventoryItem;

/// <summary>
/// Update Inventory Item command.
/// </summary>
/// <param name="Id">Inventory item ID to update</param>
/// <param name="Name">Updated name</param>
/// <param name="Description">Updated description or null</param>
public record UpdateInventoryItemCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;