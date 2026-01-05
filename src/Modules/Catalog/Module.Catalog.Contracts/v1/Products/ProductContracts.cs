using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Products;

/// <summary>
/// Command to create a new product.
/// </summary>
public record CreateProductCommand : ICommand<Guid>
{
    public required string Name { get; init; }
    public required string SKU { get; init; }
    public string? Description { get; init; }
    public string? Barcode { get; init; }
    public required decimal Price { get; init; }
    public decimal? Cost { get; init; }
    public int QuantityInStock { get; init; }
    public string? Specifications { get; init; }
    public required Guid CategoryId { get; init; }
    public required Guid BrandId { get; init; }
}

/// <summary>
/// Command to update an existing product.
/// </summary>
public record UpdateProductCommand : ICommand
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string SKU { get; init; }
    public string? Description { get; init; }
    public string? Barcode { get; init; }
    public required decimal Price { get; init; }
    public decimal? Cost { get; init; }
    public string? Specifications { get; init; }
    public required Guid CategoryId { get; init; }
    public required Guid BrandId { get; init; }
}

/// <summary>
/// Command to delete a product.
/// </summary>
public record DeleteProductCommand(Guid Id) : ICommand;

/// <summary>
/// Command to update product quantity.
/// </summary>
public record UpdateProductQuantityCommand(Guid Id, int Quantity) : ICommand;

/// <summary>
/// Query to get a product by ID.
/// </summary>
public record GetProductQuery(Guid Id) : IQuery<ProductResponse?>;

/// <summary>
/// Query to get a list of products with optional filtering.
/// </summary>
public record GetProductsQuery : IQuery<List<ProductResponse>>
{
    public string? Search { get; init; }
    public Guid? CategoryId { get; init; }
    public Guid? BrandId { get; init; }
    public bool IncludeInactive { get; init; }
}

/// <summary>
/// Response model for product details.
/// </summary>
public record ProductResponse(
    Guid Id,
    string Name,
    string SKU,
    string? Description,
    string? Barcode,
    decimal Price,
    decimal? Cost,
    int QuantityInStock,
    string? Specifications,
    Guid CategoryId,
    string CategoryName,
    Guid BrandId,
    string BrandName,
    bool IsActive,
    DateTimeOffset CreatedOnUtc,
    string? CreatedByUserName);
