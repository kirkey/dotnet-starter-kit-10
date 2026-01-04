using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a FixedAsset in the accounting system.
/// </summary>
public class FixedAsset : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    // Acquisition and valuation
    public DateTime? AcquisitionDate { get; private set; }
    public decimal Cost { get; private set; }
    public decimal ResidualValue { get; private set; }

    // Depreciation
    public decimal DepreciationRate { get; private set; } // e.g., 0.02 means 2% per period
    public decimal AccumulatedDepreciation { get; private set; }

    // Disposal
    public bool IsDisposed { get; private set; }
    public DateTime? DisposalDate { get; private set; }
    public decimal? DisposalProceeds { get; private set; }

    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;
    
    private FixedAsset() { }
    
    public static FixedAsset Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        DateTime? acquisitionDate = null,
        decimal cost = 0m,
        decimal residualValue = 0m,
        decimal depreciationRate = 0m)
    {
        if (cost < 0) throw new BadRequestException("Cost cannot be negative");
        if (residualValue < 0) throw new BadRequestException("Residual value cannot be negative");
        if (depreciationRate < 0) throw new BadRequestException("Depreciation rate cannot be negative");

        return new FixedAsset
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Description = description?.Trim(),
            AcquisitionDate = acquisitionDate?.Date,
            Cost = cost,
            ResidualValue = residualValue,
            DepreciationRate = depreciationRate,
            AccumulatedDepreciation = 0m,
            IsDisposed = false,
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }
    
    public void Update(string name, string? description = null, DateTime? acquisitionDate = null, decimal? cost = null, decimal? residualValue = null, decimal? depreciationRate = null)
    {
        if (IsDisposed)
            throw new BadRequestException("Cannot update a disposed asset");

        if (!string.IsNullOrWhiteSpace(name)) Name = name.Trim();
        Description = description?.Trim();
        AcquisitionDate = acquisitionDate?.Date ?? AcquisitionDate;
        if (cost.HasValue)
        {
            if (cost.Value < 0) throw new BadRequestException("Cost cannot be negative");
            Cost = cost.Value;
        }
        if (residualValue.HasValue)
        {
            if (residualValue.Value < 0) throw new BadRequestException("Residual value cannot be negative");
            ResidualValue = residualValue.Value;
        }
        if (depreciationRate.HasValue)
        {
            if (depreciationRate.Value < 0) throw new BadRequestException("Depreciation rate cannot be negative");
            DepreciationRate = depreciationRate.Value;
        }
    }

    public void Depreciate(decimal amount)
    {
        if (IsDisposed)
            throw new BadRequestException("Cannot depreciate a disposed asset");
        if (amount <= 0) throw new BadRequestException("Depreciation amount must be positive");

        var maxAvailable = Cost - ResidualValue - AccumulatedDepreciation;
        var toApply = Math.Min(amount, maxAvailable);
        if (toApply <= 0) throw new BadRequestException("No depreciable value remaining");

        AccumulatedDepreciation += Math.Round(toApply, 2);
    }

    public void DisposeAsset(DateTime disposalDate, decimal? proceeds = null)
    {
        if (IsDisposed)
            throw new BadRequestException("Asset already disposed");

        IsDisposed = true;
        DisposalDate = disposalDate.Date;
        DisposalProceeds = proceeds;
        IsActive = false;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
