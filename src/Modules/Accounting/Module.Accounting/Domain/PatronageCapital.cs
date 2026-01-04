namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a PatronageCapital in the accounting system.
/// </summary>
public class PatronageCapital : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;
    public Guid MemberId { get; private set; }
    public decimal AmountAllocated { get; private set; } = 0m;
    public decimal AmountRetired { get; private set; } = 0m;
    
    private PatronageCapital() { }
    
    public static PatronageCapital Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        return new PatronageCapital
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
