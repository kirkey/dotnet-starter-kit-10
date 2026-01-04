namespace FSH.Module.Microfinance.Domain;

public class LoanWriteOff : AuditableEntity<Guid>
{
    public string Name { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    
    private LoanWriteOff() { }
    
    public static LoanWriteOff Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName)
    {
        return new LoanWriteOff
        {
            Id = Guid.NewGuid(),
            Name = name,
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName
        };
    }
    
    public void Update(string name)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name;
    }
    
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
