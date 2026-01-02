namespace FSH.Framework.Core.Domain;

/// <summary>
/// Base auditable entity with common fields for all domain entities.
/// Includes audit tracking, common properties (Name, Status, IsActive, Description, Notes), and tenant support.
/// </summary>
public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity, IHasTenant
{
    // Audit fields - automatically tracked
    public DateTimeOffset CreatedOnUtc { get; set; } = DateTimeOffset.UtcNow;
    public Guid? CreatedBy { get; set; }
    public string? CreatedByUserName { get; set; }
    public DateTimeOffset? LastModifiedOnUtc { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public string? LastModifiedByUserName { get; set; }

    // Tenant support
    public string TenantId { get; set; } = default!;

    // Common entity fields - included by default in all domain entities
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = "Active";
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Updates audit information when entity is modified.
    /// </summary>
    public void SetModifiedBy(Guid userId, string userName)
    {
        LastModifiedBy = userId;
        LastModifiedByUserName = userName;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
}
