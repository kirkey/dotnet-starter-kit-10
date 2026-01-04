using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a RecurringJournalEntry in the accounting system.
/// </summary>
public class RecurringJournalEntry : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    // Scheduling
    public string Frequency { get; private set; } = "Monthly"; // Daily, Weekly, Monthly
    public DateTime? NextRunDate { get; private set; }
    public DateTime? LastRunDate { get; private set; }

    // Target period for generated journal entries
    public Guid? FiscalPeriodId { get; private set; }

    // Posting options
    public bool IsAutoPost { get; private set; } = false;
    // Approval metadata
    public Guid? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    private RecurringJournalEntry() { }
    
    public static RecurringJournalEntry Create(
        string name,
        string frequency,
        DateTime? nextRunDate,
        Guid? fiscalPeriodId,
        bool isAutoPost,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException("Name is required");
        if (!IsValidFrequency(frequency))
            throw new BadRequestException("Invalid frequency. Allowed: Daily, Weekly, Monthly");

        return new RecurringJournalEntry
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Description = description?.Trim(),
            Frequency = frequency.Trim(),
            NextRunDate = nextRunDate?.Date,
            FiscalPeriodId = fiscalPeriodId,
            IsAutoPost = isAutoPost,
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }
    
    public void Update(string name, string frequency, DateTime? nextRunDate, Guid? fiscalPeriodId, bool isAutoPost, string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name.Trim();
        if (!IsValidFrequency(frequency))
            throw new BadRequestException("Invalid frequency. Allowed: Daily, Weekly, Monthly");

        Frequency = frequency.Trim();
        NextRunDate = nextRunDate?.Date;
        FiscalPeriodId = fiscalPeriodId;
        IsAutoPost = isAutoPost;
        Description = description?.Trim();
    }

    public JournalEntry Generate(Guid createdBy, string createdByUserName)
    {
        if (!IsActive)
            throw new BadRequestException("Recurring journal entry is not active");

        if (!FiscalPeriodId.HasValue || FiscalPeriodId == Guid.Empty)
            throw new BadRequestException("Fiscal period is required to generate a journal entry");

        var now = DateTime.UtcNow.Date;
        if (NextRunDate.HasValue && NextRunDate.Value.Date > now)
            throw new BadRequestException("Recurring entry is not scheduled to run yet");

        var entryNumber = $"REC-{Id.ToString().Split('-')[0]}-{DateTime.UtcNow:yyyyMMddHHmmss}";

        var journalEntry = JournalEntry.Create(
            entryNumber,
            NextRunDate ?? now,
            "Recurring",
            Name,
            FiscalPeriodId.Value,
            TenantId,
            createdBy,
            createdByUserName,
            referenceType: "Recurring",
            description: Description);

        LastRunDate = now;
        NextRunDate = CalculateNextRun(NextRunDate ?? now);

        return journalEntry;
    }

    private DateTime CalculateNextRun(DateTime from)
    {
        return Frequency.ToLowerInvariant() switch
        {
            "daily" => from.AddDays(1),
            "weekly" => from.AddDays(7),
            "monthly" => from.AddMonths(1),
            _ => throw new BadRequestException("Invalid frequency for next run calculation")
        };
    }

    private static bool IsValidFrequency(string frequency)
    {
        var valid = new[] { "Daily", "Weekly", "Monthly" };
        return valid.Contains(frequency, StringComparer.OrdinalIgnoreCase);
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void Approve(Guid approverId)
    {
        if (ApprovedOn.HasValue)
            throw new BadRequestException("Recurring journal entry already approved");
        ApprovedBy = approverId;
        ApprovedOn = DateTime.UtcNow;
    }

    public void Reject(Guid rejectedBy)
    {
        ApprovedBy = rejectedBy;
        ApprovedOn = DateTime.UtcNow;
    }
}
