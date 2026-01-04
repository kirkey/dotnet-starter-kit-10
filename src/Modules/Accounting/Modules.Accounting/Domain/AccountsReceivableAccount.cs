namespace FSH.Modules.Accounting.Domain;

/// <summary>
/// Represents a AccountsReceivableAccount in the accounting system.
/// </summary>
public class AccountsReceivableAccount : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;
    
    private AccountsReceivableAccount() { }
    
    public static AccountsReceivableAccount Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        return new AccountsReceivableAccount
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
