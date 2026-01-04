namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a InterconnectionAgreement in the accounting system.
/// </summary>
public class InterconnectionAgreement : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public decimal LifetimeGeneration { get; private set; } = 0m;
    public decimal YearToDateGeneration { get; private set; } = 0m;
    public decimal CurrentCreditBalance { get; private set; } = 0m;
    public string Status { get; private set; } = "Draft";
    
    private InterconnectionAgreement() { }
    
    public static InterconnectionAgreement Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        return new InterconnectionAgreement
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
