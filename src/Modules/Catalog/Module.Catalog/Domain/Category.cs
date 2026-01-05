using FSH.Framework.Core.Domain;

namespace FSH.Module.Catalog.Domain;

/// <summary>
/// Represents a product category with hierarchical support.
/// 
/// **Purpose:**
/// Defines product categorization structure for organizing the catalog.
/// Supports parent-child relationships for hierarchical category trees.
/// 
/// **Business Logic:**
/// - Categories can have a parent category for hierarchical organization
/// - Categories can have multiple child categories
/// - Each category has a unique code for identification
/// - Categories can be activated/deactivated
/// 
/// **Relationships:**
/// - Self-referencing: Category → Parent Category (optional)
/// - Self-referencing: Category → Child Categories (collection)
/// - One-to-Many: Category → Products
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
    /// <summary>
    /// Gets or sets the unique code for this category.
    /// Used for identification and URL slugs.
    /// </summary>
    public string Code { get; private set; } = default!;
    
    /// <summary>
    /// Gets or sets the parent category ID (nullable for root categories).
    /// </summary>
    public Guid? ParentId { get; private set; }
    
    /// <summary>
    /// Gets or sets the parent category navigation property.
    /// </summary>
    public virtual Category? Parent { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of child categories.
    /// </summary>
    public virtual ICollection<Category> Children { get; set; } = new List<Category>();
    
    /// <summary>
    /// Gets or sets the collection of products in this category.
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    
    private Category() { } // EF Core
    
    /// <summary>
    /// Factory method to create a new Category.
    /// 
    /// Initializes a new category with default values:
    /// - IsActive: true
    /// </summary>
    /// <param name="name">The name of the category (required).</param>
    /// <param name="code">The unique code for the category (required).</param>
    /// <param name="tenantId">The tenant ID for multi-tenancy support.</param>
    /// <param name="createdBy">The user ID of the creator.</param>
    /// <param name="createdByUserName">The username of the creator.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="parentId">Optional parent category ID for hierarchical structure.</param>
    /// <returns>A new Category instance.</returns>
    /// <exception cref="ArgumentException">Thrown when name, code, or tenantId is null/whitespace.</exception>
    public static Category Create(
        string name,
        string code,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        Guid? parentId = null)
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
            ParentId = parentId,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            IsActive = true
        };
    }
    
    /// <summary>
    /// Updates the details of this category.
    /// </summary>
    /// <param name="name">New name (required).</param>
    /// <param name="code">New code (required).</param>
    /// <param name="description">New description (optional).</param>
    /// <param name="parentId">New parent category ID (optional).</param>
    /// <param name="modifiedBy">User ID making the modification.</param>
    /// <param name="modifiedByUserName">Username making the modification.</param>
    public void Update(
        string name,
        string code,
        string? description,
        Guid? parentId,
        Guid modifiedBy,
        string modifiedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        
        Name = name;
        Code = code;
        Description = description;
        ParentId = parentId;
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }
    
    /// <summary>Deactivates this category.</summary>
    public void Deactivate()
    {
        IsActive = false;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>Activates this category.</summary>
    public void Activate()
    {
        IsActive = true;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
}
