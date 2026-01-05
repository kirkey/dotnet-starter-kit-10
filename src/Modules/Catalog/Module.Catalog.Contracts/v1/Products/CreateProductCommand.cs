using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Products;

public record CreateProductCommand : ICommand<Guid>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string SKU { get; init; }
    public string? Barcode { get; init; }
    public required decimal Price { get; init; }
    public decimal? Cost { get; init; }
    public int QuantityInStock { get; init; } = 0;
    public int? ReorderLevel { get; init; }
    public required Guid CategoryId { get; init; }
    public required Guid BrandId { get; init; }
}
