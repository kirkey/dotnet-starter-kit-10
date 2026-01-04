namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a PaymentAllocation in the accounting system.
/// </summary>
public class PaymentAllocation : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;
    
    private PaymentAllocation() { }
    
    public static PaymentAllocation Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        return new PaymentAllocation
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
