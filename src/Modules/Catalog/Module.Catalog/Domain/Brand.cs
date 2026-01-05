using FSH.Framework.Core.Domain;

namespace FSH.Module.Catalog.Domain;

/// <summary>
/// Represents a product brand/manufacturer.
/// 
/// **Purpose:**
/// Represents the manufacturer or brand of products.
/// 
/// **Business Logic:**
/// - Each brand has a unique name
/// - Brands can have a website URL
/// - Brands track active/inactive status
/// 
/// **Relationships:**
/// - One-to-many: Products
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
    /// <summary>Gets or sets the website URL for this brand.</summary>
    public string? Website { get; private set; }
    
    /// <summary>Gets the collection of products from this brand.</summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    
    private Brand() { } // EF Core
    
    /// <summary>
    /// Factory method to create a new Brand.
    /// 
    /// Initializes a new brand with default values:
    /// - IsActive: true
    /// </summary>
    /// <param name="name">The brand name (required).</param>
    /// <param name="tenantId">The tenant ID for multi-tenancy support.</param>
    /// <param name="createdBy">The user ID of the creator.</param>
    /// <param name="createdByUserName">The username of the creator.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="website">Optional website URL.</param>
    /// <returns>A new Brand instance.</returns>
    public static Brand Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        string? website = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        
        return new Brand
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Website = website,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            IsActive = true
        };
    }
    
    /// <summary>
    /// Updates the brand details.
    /// </summary>
    public void Update(
        string name,
        string? description,
        string? website,
        Guid modifiedBy,
        string modifiedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        Name = name;
        Description = description;
        Website = website;
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }
    
    /// <summary>Deactivates this brand (soft delete).</summary>
    public void Deactivate()
    {
        IsActive = false;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>Activates a previously deactivated brand.</summary>
    public void Activate()
    {
        IsActive = true;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
}
