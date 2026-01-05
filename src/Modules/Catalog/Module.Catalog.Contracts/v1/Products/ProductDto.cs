namespace FSH.Module.Catalog.Contracts.v1.Products;

public record ProductDto(
    Guid Id,
    string Name,
    string? Description,
    string SKU,
    string? Barcode,
    decimal Price,
    decimal? Cost,
    int QuantityInStock,
    int? ReorderLevel,
    string Status,
    Guid CategoryId,
    string CategoryName,
    Guid BrandId,
    string BrandName,
    bool IsActive,
    DateTimeOffset CreatedOnUtc,
    string? CreatedByUserName);
