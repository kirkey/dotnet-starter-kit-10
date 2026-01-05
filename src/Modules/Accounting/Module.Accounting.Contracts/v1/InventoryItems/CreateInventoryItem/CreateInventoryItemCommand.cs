using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InventoryItems.CreateInventoryItem;

/// <summary>
/// Create Inventory Item command.
/// </summary>
/// <param name="Name">Name of the inventory item</param>
/// <param name="Description">Optional description</param>
public record CreateInventoryItemCommand(string Name, string? Description) : ICommand<Guid>;