namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a Customer in the accounting system.
/// </summary>
public class Customer : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public Guid? DefaultRateScheduleId { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    private Customer() { }
    
    public static Customer Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        Guid? defaultRateScheduleId = null)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            DefaultRateScheduleId = defaultRateScheduleId,
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
