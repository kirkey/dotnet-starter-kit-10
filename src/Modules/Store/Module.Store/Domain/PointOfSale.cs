using FSH.Framework.Core.Domain;

namespace FSH.Module.Store.Domain;

/// <summary>
/// Represents a Point of Sale (POS) terminal.
/// 
/// **Purpose:**
/// Defines POS terminal information for processing transactions in stores.
/// Simplified design for convenience stores without cash register functionality.
/// 
/// **Business Logic:**
/// - Each POS belongs to a store
/// - POS terminals have unique identifiers
/// - POS can be activated/deactivated
/// 
/// **Relationships:**
/// - Many-to-One: POS → Store
/// 
/// **Multi-Tenancy:**
/// - Supports multi-tenancy through TenantId property
/// 
/// **Audit Trail:**
/// - Tracks creation and modification details
/// - Supports soft deletes through IsActive flag
/// </summary>
public class PointOfSale : AuditableEntity<Guid>
{
    /// <summary>Gets or sets the unique identifier/code for this POS terminal.</summary>
    public string Identifier { get; private set; } = default!;
    
    /// <summary>Gets or sets the location within the store (optional).</summary>
    public string? Location { get; private set; }
    
    /// <summary>Gets or sets the store ID this POS belongs to.</summary>
    public Guid StoreId { get; private set; }
    
    /// <summary>Gets or sets the store navigation property.</summary>
    public virtual Store Store { get; set; } = default!;
    
    private PointOfSale() { } // EF Core
    
    /// <summary>
    /// Factory method to create a new POS terminal.
    /// </summary>
    public static PointOfSale Create(
        string name,
        string identifier,
        Guid storeId,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        string? location = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        
        if (storeId == Guid.Empty)
            throw new ArgumentException("Store ID is required", nameof(storeId));
        
        return new PointOfSale
        {
            Id = Guid.NewGuid(),
            Name = name,
            Identifier = identifier,
            Description = description,
            Location = location,
            StoreId = storeId,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            IsActive = true
        };
    }
    
    /// <summary>
    /// Updates the details of this POS terminal.
    /// </summary>
    public void Update(
        string name,
        string identifier,
        string? description,
        string? location,
        Guid storeId,
        Guid modifiedBy,
        string modifiedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        
        if (storeId == Guid.Empty)
            throw new ArgumentException("Store ID is required", nameof(storeId));
        
        Name = name;
        Identifier = identifier;
        Description = description;
        Location = location;
        StoreId = storeId;
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }
    
    /// <summary>Deactivates this POS terminal.</summary>
    public void Deactivate()
    {
        IsActive = false;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>Activates this POS terminal.</summary>
    public void Activate()
    {
        IsActive = true;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
}
