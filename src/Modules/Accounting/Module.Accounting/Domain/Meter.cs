namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a Meter in the accounting system.
/// </summary>
public class Meter : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public Guid? MemberId { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    private Meter() { }
    
    public static Meter Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        Guid? memberId = null)
    {
        return new Meter
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            MemberId = memberId,
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
