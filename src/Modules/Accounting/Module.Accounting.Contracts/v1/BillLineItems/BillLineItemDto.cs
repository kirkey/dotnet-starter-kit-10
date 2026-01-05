namespace FSH.Module.Accounting.Contracts.v1.BillLineItems;

public record BillLineItemDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record BillLineItemSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);