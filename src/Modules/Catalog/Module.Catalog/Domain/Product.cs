using FSH.Framework.Core.Domain;

namespace FSH.Module.Catalog.Domain;

/// <summary>
/// Represents a product in the catalog.
/// 
/// **Purpose:**
/// Core entity representing a product with pricing, inventory, and categorization.
/// 
/// **Business Logic:**
/// - Each product must belong to a category and brand
/// - Products have a unique SKU (Stock Keeping Unit)
/// - Products track price and inventory quantity
/// - Products have status (Draft, Active, OutOfStock, Discontinued)
/// 
/// **Relationships:**
/// - Many-to-one: Category
/// - Many-to-one: Brand
/// 
/// **Multi-Tenancy:**
/// - Supports multi-tenancy through TenantId property
/// 
/// **Audit Trail:**
/// - Tracks creation and modification details
/// - Supports soft deletes through IsActive flag
/// </summary>
public class Product : AuditableEntity<Guid>
{
    /// <summary>Gets or sets the Stock Keeping Unit (unique identifier).</summary>
    public string SKU { get; private set; } = default!;
    
    /// <summary>Gets or sets the barcode for this product.</summary>
    public string? Barcode { get; private set; }
    
    /// <summary>Gets or sets the price of this product.</summary>
    public decimal Price { get; private set; }
    
    /// <summary>Gets or sets the cost of this product.</summary>
    public decimal? Cost { get; private set; }
    
    /// <summary>Gets or sets the quantity in stock.</summary>
    public int QuantityInStock { get; private set; }
    
    /// <summary>Gets or sets the reorder level for stock alerts.</summary>
    public int? ReorderLevel { get; private set; }
    
    /// <summary>Gets or sets the category ID.</summary>
    public Guid CategoryId { get; private set; }
    
    /// <summary>Gets the category navigation property.</summary>
    public virtual Category Category { get; set; } = default!;
    
    /// <summary>Gets or sets the brand ID.</summary>
    public Guid BrandId { get; private set; }
    
    /// <summary>Gets the brand navigation property.</summary>
    public virtual Brand Brand { get; set; } = default!;
    
    private Product() { } // EF Core
    
    /// <summary>
    /// Factory method to create a new Product.
    /// 
    /// Initializes a new product with default values:
    /// - Status: Draft
    /// - IsActive: true
    /// - QuantityInStock: 0
    /// </summary>
    /// <param name="name">The product name (required).</param>
    /// <param name="sku">The SKU (required, unique).</param>
    /// <param name="price">The product price.</param>
    /// <param name="categoryId">The category ID.</param>
    /// <param name="brandId">The brand ID.</param>
    /// <param name="tenantId">The tenant ID for multi-tenancy support.</param>
    /// <param name="createdBy">The user ID of the creator.</param>
    /// <param name="createdByUserName">The username of the creator.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="barcode">Optional barcode.</param>
    /// <param name="cost">Optional cost.</param>
    /// <param name="quantityInStock">Initial quantity in stock (default: 0).</param>
    /// <param name="reorderLevel">Optional reorder level.</param>
    /// <returns>A new Product instance.</returns>
    public static Product Create(
        string name,
        string sku,
        decimal price,
        Guid categoryId,
        Guid brandId,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        string? barcode = null,
        decimal? cost = null,
        int quantityInStock = 0,
        int? reorderLevel = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        
        if (cost.HasValue && cost.Value < 0)
            throw new ArgumentException("Cost cannot be negative.", nameof(cost));
        
        if (quantityInStock < 0)
            throw new ArgumentException("Quantity in stock cannot be negative.", nameof(quantityInStock));
        
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            SKU = sku,
            Barcode = barcode,
            Price = price,
            Cost = cost,
            QuantityInStock = quantityInStock,
            ReorderLevel = reorderLevel,
            CategoryId = categoryId,
            BrandId = brandId,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            Status = nameof(ProductStatus.Draft),
            IsActive = true
        };
    }
    
    /// <summary>
    /// Updates the product details.
    /// </summary>
    public void Update(
        string name,
        string sku,
        decimal price,
        Guid categoryId,
        Guid brandId,
        string? description,
        string? barcode,
        decimal? cost,
        int quantityInStock,
        int? reorderLevel,
        Guid modifiedBy,
        string modifiedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        
        if (cost.HasValue && cost.Value < 0)
            throw new ArgumentException("Cost cannot be negative.", nameof(cost));
        
        if (quantityInStock < 0)
            throw new ArgumentException("Quantity in stock cannot be negative.", nameof(quantityInStock));
        
        Name = name;
        Description = description;
        SKU = sku;
        Barcode = barcode;
        Price = price;
        Cost = cost;
        QuantityInStock = quantityInStock;
        ReorderLevel = reorderLevel;
        CategoryId = categoryId;
        BrandId = brandId;
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }
    
    /// <summary>
    /// Updates the product status.
    /// </summary>
    public void UpdateStatus(ProductStatus status)
    {
        Status = status.ToString();
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>Updates the quantity in stock.</summary>
    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity in stock cannot be negative.", nameof(quantity));
        
        QuantityInStock = quantity;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
        
        // Auto-update status if out of stock
        if (quantity == 0 && Status == nameof(ProductStatus.Active))
        {
            Status = nameof(ProductStatus.OutOfStock);
        }
        else if (quantity > 0 && Status == nameof(ProductStatus.OutOfStock))
        {
            Status = nameof(ProductStatus.Active);
        }
    }
    
    /// <summary>Deactivates this product (soft delete).</summary>
    public void Deactivate()
    {
        IsActive = false;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>Activates a previously deactivated product.</summary>
    public void Activate()
    {
        IsActive = true;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
}
