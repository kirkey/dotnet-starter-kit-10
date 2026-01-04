namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a AccountReconciliation in the accounting system.
/// </summary>
public class AccountReconciliation : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public Guid? AccountingPeriodId { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    private AccountReconciliation() { }
    
    public static AccountReconciliation Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        Guid? accountingPeriodId = null)
    {
        return new AccountReconciliation
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            AccountingPeriodId = accountingPeriodId,
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
