using FSH.Framework.Core.Domain;

namespace FSH.Module.Store.Domain;

/// <summary>
/// Represents a physical store location.
/// 
/// **Purpose:**
/// Defines store information including location, contact details, and operational status.
/// Designed for small to medium-sized convenience stores.
/// 
/// **Business Logic:**
/// - Each store has a unique name and address
/// - Stores can have multiple POS terminals
/// - Stores can be activated/deactivated
/// 
/// **Relationships:**
/// - One-to-Many: Store → POS terminals
/// 
/// **Multi-Tenancy:**
/// - Supports multi-tenancy through TenantId property
/// 
/// **Audit Trail:**
/// - Tracks creation and modification details
/// - Supports soft deletes through IsActive flag
/// </summary>
public class Store : AuditableEntity<Guid>
{
    /// <summary>Gets or sets the store address.</summary>
    public string Address { get; private set; } = default!;
    
    /// <summary>Gets or sets the city.</summary>
    public string City { get; private set; } = default!;
    
    /// <summary>Gets or sets the state/province (optional).</summary>
    public string? State { get; private set; }
    
    /// <summary>Gets or sets the postal code.</summary>
    public string PostalCode { get; private set; } = default!;
    
    /// <summary>Gets or sets the phone number (optional).</summary>
    public string? Phone { get; private set; }
    
    /// <summary>Gets or sets the email address (optional).</summary>
    public string? Email { get; private set; }
    
    /// <summary>Gets or sets the collection of POS terminals at this store.</summary>
    public virtual ICollection<PointOfSale> POSTerminals { get; set; } = new List<PointOfSale>();
    
    private Store() { } // EF Core
    
    /// <summary>
    /// Factory method to create a new Store.
    /// </summary>
    public static Store Create(
        string name,
        string address,
        string city,
        string postalCode,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        string? state = null,
        string? phone = null,
        string? email = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(address);
        ArgumentException.ThrowIfNullOrWhiteSpace(city);
        ArgumentException.ThrowIfNullOrWhiteSpace(postalCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        
        return new Store
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Address = address,
            City = city,
            State = state,
            PostalCode = postalCode,
            Phone = phone,
            Email = email,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            IsActive = true
        };
    }
    
    /// <summary>
    /// Updates the details of this store.
    /// </summary>
    public void Update(
        string name,
        string address,
        string city,
        string postalCode,
        string? description,
        string? state,
        string? phone,
        string? email,
        Guid modifiedBy,
        string modifiedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(address);
        ArgumentException.ThrowIfNullOrWhiteSpace(city);
        ArgumentException.ThrowIfNullOrWhiteSpace(postalCode);
        
        Name = name;
        Description = description;
        Address = address;
        City = city;
        State = state;
        PostalCode = postalCode;
        Phone = phone;
        Email = email;
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }
    
    /// <summary>Deactivates this store.</summary>
    public void Deactivate()
    {
        IsActive = false;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>Activates this store.</summary>
    public void Activate()
    {
        IsActive = true;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
}
