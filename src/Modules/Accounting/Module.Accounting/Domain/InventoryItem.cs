using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a InventoryItem in the accounting system.
/// </summary>
public class InventoryItem : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public decimal Quantity { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    private InventoryItem() { }
    
    public static InventoryItem Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        decimal quantity = 0m)
    {
        return new InventoryItem
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Quantity = quantity,
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
    
    public void AddStock(decimal quantity)
    {
        if (quantity <= 0)
            throw new BadRequestException("Quantity to add must be positive");
        
        Quantity += quantity;
    }
    
    public void ReduceStock(decimal quantity)
    {
        if (quantity <= 0)
            throw new BadRequestException("Quantity to reduce must be positive");
        
        if (Quantity < quantity)
            throw new BadRequestException("Insufficient stock available");
        
        Quantity -= quantity;
    }
    
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
