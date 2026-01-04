namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a CreditMemo in the accounting system.
/// </summary>
public class CreditMemo : AuditableEntity<Guid>, IMustHaveTenant
{
    
    private CreditMemo() { }
    
    public static CreditMemo Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        return new CreditMemo
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
