using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Store.Exceptions;

/// <summary>
/// Thrown when a store is not found in the database.
/// </summary>
public class StoreNotFoundException : NotFoundException
{
    public StoreNotFoundException(Guid id)
        : base($"Store with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Thrown when a POS terminal is not found in the database.
/// </summary>
public class POSNotFoundException : NotFoundException
{
    public POSNotFoundException(Guid id)
        : base($"POS terminal with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Extension methods for common exception scenarios in Store operations.
/// </summary>
public static class StoreExceptionExtensions
{
    /// <summary>
    /// Throws StoreNotFoundException if entity is null.
    /// </summary>
    public static Store ThrowIfNotFound(this Store? store, Guid id)
        => store ?? throw new StoreNotFoundException(id);
    
    /// <summary>
    /// Throws POSNotFoundException if entity is null.
    /// </summary>
    public static PointOfSale ThrowIfNotFound(this PointOfSale? pos, Guid id)
        => pos ?? throw new POSNotFoundException(id);
}
