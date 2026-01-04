namespace Accounting.Application.InventoryItems.Update.v1;

public sealed record UpdateInventoryItemCommand(
    DefaultIdType Id,
    string? Sku,
    string? Name,
    decimal? Quantity,
    decimal? UnitPrice,
    string? Description,
    string? ImageUrl
) : IRequest<DefaultIdType>;

