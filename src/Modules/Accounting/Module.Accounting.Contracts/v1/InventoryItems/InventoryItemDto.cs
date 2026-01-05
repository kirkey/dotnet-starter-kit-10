namespace FSH.Module.Accounting.Contracts.v1.InventoryItems;

public record InventoryItemDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record InventoryItemSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);