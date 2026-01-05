using FSH.Framework.Core.Domain;

namespace FSH.Module.Catalog.Domain;

/// <summary>
/// Represents a product category with hierarchical support.
/// 
/// **Purpose:**
/// Organizes products into logical groups with support for parent-child relationships.
/// 
/// **Business Logic:**
/// - Categories can have parent categories (hierarchical structure)
/// - Each category has a unique code for identification
/// - Categories track active/inactive status
/// 
/// **Relationships:**
/// - Self-referential: Parent category
/// - One-to-many: Products
/// 
/// **Multi-Tenancy:**
/// - Supports multi-tenancy through TenantId property
/// 
/// **Audit Trail:**
/// - Tracks creation and modification details
/// - Supports soft deletes through IsActive flag
/// </summary>
public class Category : AuditableEntity<Guid>
{
    /// <summary>Gets or sets the unique code for this category.</summary>
    public string Code { get; private set; } = default!;
    
    /// <summary>Gets or sets the parent category ID (nullable for root categories).</summary>
    public Guid? ParentCategoryId { get; private set; }
    
    /// <summary>Gets the parent category navigation property.</summary>
    public virtual Category? ParentCategory { get; set; }
    
    /// <summary>Gets the collection of child categories.</summary>
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    
    /// <summary>Gets the collection of products in this category.</summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    
    private Category() { } // EF Core
    
    /// <summary>
    /// Factory method to create a new Category.
    /// 
    /// Initializes a new category with default values:
    /// - IsActive: true
    /// </summary>
    /// <param name="name">The category name (required).</param>
    /// <param name="code">The unique category code (required).</param>
    /// <param name="tenantId">The tenant ID for multi-tenancy support.</param>
    /// <param name="createdBy">The user ID of the creator.</param>
    /// <param name="createdByUserName">The username of the creator.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="parentCategoryId">Optional parent category ID for hierarchical categories.</param>
    /// <returns>A new Category instance.</returns>
    public static Category Create(
        string name,
        string code,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        Guid? parentCategoryId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            Code = code,
            Description = description,
            ParentCategoryId = parentCategoryId,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            IsActive = true
        };
    }
    
    /// <summary>
    /// Updates the category details.
    /// </summary>
    public void Update(
        string name,
        string code,
        string? description,
        Guid? parentCategoryId,
        Guid modifiedBy,
        string modifiedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        
        Name = name;
        Code = code;
        Description = description;
        ParentCategoryId = parentCategoryId;
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }
    
    /// <summary>Deactivates this category (soft delete).</summary>
    public void Deactivate()
    {
        IsActive = false;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>Activates a previously deactivated category.</summary>
    public void Activate()
    {
        IsActive = true;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
}
