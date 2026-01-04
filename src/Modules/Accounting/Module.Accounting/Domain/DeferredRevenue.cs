namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a DeferredRevenue in the accounting system.
/// </summary>
public class DeferredRevenue : AuditableEntity<Guid>, IMustHaveTenant
{
    
    private DeferredRevenue() { }
    
    public static DeferredRevenue Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        return new DeferredRevenue
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }
    
    public void Update(string name, string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name;
        Description = description;
    }
    
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
