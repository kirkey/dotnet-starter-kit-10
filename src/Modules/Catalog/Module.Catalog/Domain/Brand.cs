using FSH.Framework.Core.Domain;

namespace FSH.Module.Catalog.Domain;

/// <summary>
/// Represents a product brand or manufacturer.
/// 
/// **Purpose:**
/// Defines brand information for products in the catalog.
/// Tracks brand details including website and contact information.
/// 
/// **Business Logic:**
/// - Each brand has a unique name
/// - Brands can have an associated website URL
/// - Brands can be activated/deactivated
/// 
/// **Relationships:**
/// - One-to-Many: Brand → Products
/// 
/// **Multi-Tenancy:**
/// - Supports multi-tenancy through TenantId property
/// 
/// **Audit Trail:**
/// - Tracks creation and modification details
/// - Supports soft deletes through IsActive flag
/// </summary>
public class Brand : AuditableEntity<Guid>
{
    /// <summary>
    /// Gets or sets the brand's website URL (optional).
    /// </summary>
    public string? WebsiteUrl { get; private set; }
    
    /// <summary>
    /// Gets or sets the collection of products for this brand.
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    
    private Brand() { } // EF Core
    
    /// <summary>
    /// Factory method to create a new Brand.
    /// 
    /// Initializes a new brand with default values:
    /// - IsActive: true
    /// </summary>
    /// <param name="name">The name of the brand (required).</param>
    /// <param name="tenantId">The tenant ID for multi-tenancy support.</param>
    /// <param name="createdBy">The user ID of the creator.</param>
    /// <param name="createdByUserName">The username of the creator.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="websiteUrl">Optional website URL.</param>
    /// <returns>A new Brand instance.</returns>
    /// <exception cref="ArgumentException">Thrown when name or tenantId is null/whitespace.</exception>
    public static Brand Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        string? websiteUrl = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        
        return new Brand
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            WebsiteUrl = websiteUrl,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            IsActive = true
        };
    }
    
    /// <summary>
    /// Updates the details of this brand.
    /// </summary>
    /// <param name="name">New name (required).</param>
    /// <param name="description">New description (optional).</param>
    /// <param name="websiteUrl">New website URL (optional).</param>
    /// <param name="modifiedBy">User ID making the modification.</param>
    /// <param name="modifiedByUserName">Username making the modification.</param>
    public void Update(
        string name,
        string? description,
        string? websiteUrl,
        Guid modifiedBy,
        string modifiedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        Name = name;
        Description = description;
        WebsiteUrl = websiteUrl;
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }
    
    /// <summary>Deactivates this brand.</summary>
    public void Deactivate()
    {
        IsActive = false;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>Activates this brand.</summary>
    public void Activate()
    {
        IsActive = true;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
}
