namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a DebitMemo in the accounting system.
/// </summary>
public class DebitMemo : AuditableEntity<Guid>, IMustHaveTenant
{
    
    private DebitMemo() { }
    
    public static DebitMemo Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        return new DebitMemo
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
