using FSH.Framework.Core.Domain;

namespace FSH.Module.Catalog.Domain;

/// <summary>
/// Represents a product in the catalog.
/// 
/// **Purpose:**
/// Defines product information including pricing, inventory, and relationships
/// to categories and brands.
/// 
/// **Business Logic:**
/// - Each product has a unique SKU (Stock Keeping Unit)
/// - Products belong to a category and brand
/// - Products have pricing and cost information
/// - Products track inventory quantity
/// - Products can have barcodes for scanning
/// - Products can be activated/deactivated
/// 
/// **Relationships:**
/// - Many-to-One: Product → Category
/// - Many-to-One: Product → Brand
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
    /// <summary>
    /// Gets or sets the SKU (Stock Keeping Unit) for this product.
    /// Unique identifier for inventory management.
    /// </summary>
    public string SKU { get; private set; } = default!;
    
    /// <summary>
    /// Gets or sets the barcode for this product (optional).
    /// Used for barcode scanning and POS systems.
    /// </summary>
    public string? Barcode { get; private set; }
    
    /// <summary>
    /// Gets or sets the retail price of the product.
    /// </summary>
    public decimal Price { get; private set; }
    
    /// <summary>
    /// Gets or sets the cost price of the product (optional).
    /// Used for profit margin calculations.
    /// </summary>
    public decimal? Cost { get; private set; }
    
    /// <summary>
    /// Gets or sets the quantity in stock.
    /// </summary>
    public int QuantityInStock { get; private set; }
    
    /// <summary>
    /// Gets or sets the technical specifications of the product (optional).
    /// </summary>
    public string? Specifications { get; private set; }
    
    /// <summary>
    /// Gets or sets the category ID this product belongs to.
    /// </summary>
    public Guid CategoryId { get; private set; }
    
    /// <summary>
    /// Gets or sets the brand ID this product belongs to.
    /// </summary>
    public Guid BrandId { get; private set; }
    
    /// <summary>
    /// Gets or sets the category navigation property.
    /// </summary>
    public virtual Category Category { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the brand navigation property.
    /// </summary>
    public virtual Brand Brand { get; set; } = default!;
    
    private Product() { } // EF Core
    
    /// <summary>
    /// Factory method to create a new Product.
    /// 
    /// Initializes a new product with default values:
    /// - IsActive: true
    /// - QuantityInStock: 0
    /// </summary>
    /// <param name="name">The name of the product (required).</param>
    /// <param name="sku">The SKU (Stock Keeping Unit) (required).</param>
    /// <param name="price">The retail price (must be non-negative).</param>
    /// <param name="categoryId">The category ID (required).</param>
    /// <param name="brandId">The brand ID (required).</param>
    /// <param name="tenantId">The tenant ID for multi-tenancy support.</param>
    /// <param name="createdBy">The user ID of the creator.</param>
    /// <param name="createdByUserName">The username of the creator.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="barcode">Optional barcode.</param>
    /// <param name="cost">Optional cost price.</param>
    /// <param name="quantityInStock">Initial stock quantity (default: 0).</param>
    /// <param name="specifications">Optional specifications.</param>
    /// <returns>A new Product instance.</returns>
    /// <exception cref="ArgumentException">Thrown when required parameters are null/whitespace or invalid.</exception>
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
        string? specifications = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        
        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));
        
        if (cost.HasValue && cost.Value < 0)
            throw new ArgumentException("Cost cannot be negative", nameof(cost));
        
        if (quantityInStock < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantityInStock));
        
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category ID is required", nameof(categoryId));
        
        if (brandId == Guid.Empty)
            throw new ArgumentException("Brand ID is required", nameof(brandId));
        
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            SKU = sku,
            Description = description,
            Barcode = barcode,
            Price = price,
            Cost = cost,
            QuantityInStock = quantityInStock,
            Specifications = specifications,
            CategoryId = categoryId,
            BrandId = brandId,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            IsActive = true
        };
    }
    
    /// <summary>
    /// Updates the details of this product.
    /// </summary>
    /// <param name="name">New name (required).</param>
    /// <param name="sku">New SKU (required).</param>
    /// <param name="description">New description (optional).</param>
    /// <param name="barcode">New barcode (optional).</param>
    /// <param name="price">New price.</param>
    /// <param name="cost">New cost (optional).</param>
    /// <param name="specifications">New specifications (optional).</param>
    /// <param name="categoryId">New category ID.</param>
    /// <param name="brandId">New brand ID.</param>
    /// <param name="modifiedBy">User ID making the modification.</param>
    /// <param name="modifiedByUserName">Username making the modification.</param>
    public void Update(
        string name,
        string sku,
        string? description,
        string? barcode,
        decimal price,
        decimal? cost,
        string? specifications,
        Guid categoryId,
        Guid brandId,
        Guid modifiedBy,
        string modifiedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        
        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));
        
        if (cost.HasValue && cost.Value < 0)
            throw new ArgumentException("Cost cannot be negative", nameof(cost));
        
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category ID is required", nameof(categoryId));
        
        if (brandId == Guid.Empty)
            throw new ArgumentException("Brand ID is required", nameof(brandId));
        
        Name = name;
        SKU = sku;
        Description = description;
        Barcode = barcode;
        Price = price;
        Cost = cost;
        Specifications = specifications;
        CategoryId = categoryId;
        BrandId = brandId;
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }
    
    /// <summary>
    /// Updates the inventory quantity for this product.
    /// </summary>
    /// <param name="quantity">New quantity (must be non-negative).</param>
    public void UpdateQuantity(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantity));
        
        QuantityInStock = quantity;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>Deactivates this product.</summary>
    public void Deactivate()
    {
        IsActive = false;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>Activates this product.</summary>
    public void Activate()
    {
        IsActive = true;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
}
