using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Catalog.Exceptions;

/// <summary>
/// Thrown when a category is not found in the database.
/// </summary>
public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(Guid id)
        : base($"Category with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Thrown when a brand is not found in the database.
/// </summary>
public class BrandNotFoundException : NotFoundException
{
    public BrandNotFoundException(Guid id)
        : base($"Brand with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Thrown when a product is not found in the database.
/// </summary>
public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid id)
        : base($"Product with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Extension methods for common exception scenarios in Catalog operations.
/// Centralizes exception creation to avoid duplication across handlers.
/// </summary>
public static class CatalogExceptionExtensions
{
    /// <summary>
    /// Throws CategoryNotFoundException if entity is null.
    /// </summary>
    public static Category ThrowIfNotFound(this Category? category, Guid id)
        => category ?? throw new CategoryNotFoundException(id);
    
    /// <summary>
    /// Throws BrandNotFoundException if entity is null.
    /// </summary>
    public static Brand ThrowIfNotFound(this Brand? brand, Guid id)
        => brand ?? throw new BrandNotFoundException(id);
    
    /// <summary>
    /// Throws ProductNotFoundException if entity is null.
    /// </summary>
    public static Product ThrowIfNotFound(this Product? product, Guid id)
        => product ?? throw new ProductNotFoundException(id);
}
