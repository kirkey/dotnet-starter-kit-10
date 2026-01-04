using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a RetainedEarnings in the accounting system.
/// </summary>
public class RetainedEarnings : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    // Fiscal info
    public int? FiscalYear { get; private set; }
    public decimal OpeningBalance { get; private set; }
    public decimal ClosingBalance { get; private set; }
    public bool IsClosed { get; private set; }
    public DateTime? ClosedOn { get; private set; }
    public Guid? ClosedBy { get; private set; }

    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;
    
    private RetainedEarnings() { }
    
    public static RetainedEarnings Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        return new RetainedEarnings
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            IsClosed = false,
            OpeningBalance = 0m,
            ClosingBalance = 0m
        };
    }
    
    public void Update(string name, string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name;
        Description = description;
    }

    public void Close(int fiscalYear, decimal closingBalance, Guid closedBy)
    {
        if (IsClosed)
            throw new BadRequestException("Retained earnings already closed for the period");
        if (fiscalYear <= 0)
            throw new BadRequestException("Fiscal year is required");
        if (closingBalance < 0)
            throw new BadRequestException("Closing balance cannot be negative");

        FiscalYear = fiscalYear;
        ClosingBalance = closingBalance;
        IsClosed = true;
        ClosedOn = DateTime.UtcNow;
        ClosedBy = closedBy;
    }

    public void Reopen(string reason)
    {
        if (!IsClosed)
            throw new BadRequestException("Retained earnings are not closed");

        // Keep last close info but mark as reopened
        IsClosed = false;
        ClosedOn = null;
        ClosedBy = null;
        // Optionally record reason as an audit/event outside entity
    }
    
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
