namespace FSH.Module.Catalog.Domain;

/// <summary>
/// Represents the availability status of a product.
/// </summary>
public enum ProductStatus
{
    /// <summary>Product is in draft mode, not yet available for sale.</summary>
    Draft = 0,
    
    /// <summary>Product is active and available for sale.</summary>
    Active = 1,
    
    /// <summary>Product is temporarily out of stock.</summary>
    OutOfStock = 2,
    
    /// <summary>Product has been discontinued.</summary>
    Discontinued = 3
}
