namespace FSH.Modules.Microfinance.Domain;

public class MarketingCampaign : AuditableEntity<Guid>
{
    public string Name { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    
    private MarketingCampaign() { }
    
    public static MarketingCampaign Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName)
    {
        return new MarketingCampaign
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
