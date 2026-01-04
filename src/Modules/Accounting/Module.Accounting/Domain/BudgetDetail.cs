namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a BudgetDetail in the accounting system.
/// </summary>
public class BudgetDetail : AuditableEntity<Guid>, IMustHaveTenant
{
    
    private BudgetDetail() { }
    
    public static BudgetDetail Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        return new BudgetDetail
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
