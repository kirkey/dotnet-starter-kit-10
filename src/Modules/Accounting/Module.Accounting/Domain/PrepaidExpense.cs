namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a PrepaidExpense in the accounting system.
/// </summary>
public class PrepaidExpense : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public Guid? CostCenterId { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    private PrepaidExpense() { }
    
    public static PrepaidExpense Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        Guid? costCenterId = null)
    {
        return new PrepaidExpense
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CostCenterId = costCenterId,
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
