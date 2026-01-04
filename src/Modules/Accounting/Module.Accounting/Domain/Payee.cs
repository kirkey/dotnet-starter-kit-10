namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a Payee in the accounting system.
/// </summary>
public class Payee : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;
    
    private Payee() { }
    
    public static Payee Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        return new Payee
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
