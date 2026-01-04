namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a FiscalPeriodClose in the accounting system.
/// </summary>
public class FiscalPeriodClose : AuditableEntity<Guid>, IMustHaveTenant
{
    public Guid FiscalPeriodId { get; private set; }
    public int FiscalYear { get; private set; }
    public string PeriodName { get; private set; } = default!; // e.g., "2025-Q1" or "2025-01"
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public DateTime? CloseDate { get; private set; }
    public string Status { get; private set; } = "Open"; // Open, InClosing, Closed, Reopened
    public bool IsClosed => Status == "Closed";

    public decimal RetainedEarnings { get; private set; }
    public Guid? ClosingJournalEntryId { get; private set; }
    public Guid? ClosedBy { get; private set; }

    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;

    private FiscalPeriodClose() { }

    public static FiscalPeriodClose Create(
        Guid fiscalPeriodId,
        int fiscalYear,
        string periodName,
        DateTime startDate,
        DateTime endDate,
        decimal retainedEarnings,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        if (fiscalPeriodId == Guid.Empty)
            throw new BadRequestException("Fiscal period is required");
        if (string.IsNullOrWhiteSpace(periodName))
            throw new BadRequestException("Period name is required");
        if (endDate < startDate)
            throw new BadRequestException("End date cannot be before start date");

        return new FiscalPeriodClose
        {
            Id = Guid.NewGuid(),
            FiscalPeriodId = fiscalPeriodId,
            FiscalYear = fiscalYear,
            PeriodName = periodName.Trim(),
            StartDate = startDate.Date,
            EndDate = endDate.Date,
            RetainedEarnings = retainedEarnings,
            Status = "Open",
            Description = description?.Trim(),
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }

    public void Update(
        DateTime startDate,
        DateTime endDate,
        decimal retainedEarnings,
        string? description = null)
    {
        if (Status == "Closed")
            throw new BadRequestException("Cannot update a closed fiscal period");
        if (endDate < startDate)
            throw new BadRequestException("End date cannot be before start date");

        StartDate = startDate.Date;
        EndDate = endDate.Date;
        RetainedEarnings = retainedEarnings;
        Description = description?.Trim();
    }

    public void BeginClose()
    {
        if (Status != "Open")
            throw new BadRequestException("Fiscal period must be open to initiate close");
        Status = "InClosing";
    }

    public void ApplyAdjustment(decimal amount, string? notes = null)
    {
        RetainedEarnings += amount;
        Description = notes ?? Description;
    }

    public void CompleteClose(Guid closedBy, Guid closingJournalEntryId, DateTime? closeDate = null)
    {
        if (Status != "InClosing")
            throw new BadRequestException("Fiscal period must be in closing state to complete close");
        if (closingJournalEntryId == Guid.Empty)
            throw new BadRequestException("Closing journal entry id is required");

        Status = "Closed";
        ClosingJournalEntryId = closingJournalEntryId;
        CloseDate = closeDate ?? DateTime.UtcNow;
        ClosedBy = closedBy;
        IsActive = false; // make closed periods read-only
    }

    public void Reopen()
    {
        if (Status != "Closed")
            throw new BadRequestException("Only closed periods can be reopened");
        Status = "Reopened";
        IsActive = true;
        CloseDate = null;
        ClosedBy = null;
        ClosingJournalEntryId = null;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
