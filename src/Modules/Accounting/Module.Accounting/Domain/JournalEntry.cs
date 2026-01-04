namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a Journal Entry in the accounting system.
/// A journal entry records debits and credits to various accounts.
/// </summary>
public class JournalEntry : AuditableEntity<Guid>, IMustHaveTenant
{
    // Core Properties
    public string EntryNumber { get; private set; } = default!;
    public DateTime EntryDate { get; private set; }
    public string EntryType { get; private set; } = default!; // Standard, Adjusting, Closing, Reversing
    public string ReferenceNumber { get; private set; } = default!;
    public string? ReferenceType { get; private set; } // Invoice, Payment, Manual, etc.
    
    // Financial Properties
    public decimal TotalDebit { get; private set; }
    public decimal TotalCredit { get; private set; }
    
    // Period & Status
    public Guid FiscalPeriodId { get; private set; }
    public string Status { get; private set; } = "Draft"; // Draft, Posted, Approved, Voided
    public DateTime? PostedDate { get; private set; }
    public Guid? PostedBy { get; private set; }
    public DateTime? ApprovedDate { get; private set; }
    public Guid? ApprovedBy { get; private set; }
    
    // Reversal
    public bool IsReversed { get; private set; }
    public Guid? ReversedEntryId { get; private set; }
    public DateTime? ReversedDate { get; private set; }
    
    // Documentation
    public string? Description { get; private set; }
    public string? Notes { get; private set; }
    public string? Memo { get; private set; }
    
    // Audit & Status
    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;
    
    // Navigation Properties (not mapped by default)
    public ICollection<JournalEntryLine>? Lines { get; private set; }
    
    private JournalEntry() { }
    
    public static JournalEntry Create(
        string entryNumber,
        DateTime entryDate,
        string entryType,
        string referenceNumber,
        Guid fiscalPeriodId,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? referenceType = null,
        string? description = null,
        string? notes = null,
        string? memo = null)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(entryNumber))
            throw new BadRequestException("Entry number is required");
        if (string.IsNullOrWhiteSpace(referenceNumber))
            throw new BadRequestException("Reference number is required");
        if (!IsValidEntryType(entryType))
            throw new BadRequestException("Invalid entry type. Must be Standard, Adjusting, Closing, or Reversing");
        if (fiscalPeriodId == Guid.Empty)
            throw new BadRequestException("Fiscal period is required");
        
        return new JournalEntry
        {
            Id = Guid.NewGuid(),
            EntryNumber = entryNumber.Trim(),
            EntryDate = entryDate.Date,
            EntryType = entryType.Trim(),
            ReferenceNumber = referenceNumber.Trim(),
            ReferenceType = referenceType?.Trim(),
            FiscalPeriodId = fiscalPeriodId,
            TotalDebit = 0,
            TotalCredit = 0,
            Status = "Draft",
            IsReversed = false,
            Description = description?.Trim(),
            Notes = notes?.Trim(),
            Memo = memo?.Trim(),
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }
    
    public void Update(
        string entryNumber,
        DateTime entryDate,
        string entryType,
        string referenceNumber,
        Guid fiscalPeriodId,
        string? referenceType = null,
        string? description = null,
        string? notes = null,
        string? memo = null)
    {
        if (Status == "Posted" || Status == "Approved")
            throw new BadRequestException("Cannot update a posted or approved journal entry");
        if (IsReversed)
            throw new BadRequestException("Cannot update a reversed journal entry");
        
        if (!string.IsNullOrWhiteSpace(entryNumber)) EntryNumber = entryNumber.Trim();
        if (!IsValidEntryType(entryType))
            throw new BadRequestException("Invalid entry type");
        
        EntryDate = entryDate.Date;
        EntryType = entryType.Trim();
        ReferenceNumber = referenceNumber.Trim();
        FiscalPeriodId = fiscalPeriodId;
        ReferenceType = referenceType?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();
        Memo = memo?.Trim();
    }
    
    public void UpdateTotals(decimal totalDebit, decimal totalCredit)
    {
        TotalDebit = totalDebit;
        TotalCredit = totalCredit;
    }
    
    public void Post(Guid postedBy, DateTime? postedDate = null)
    {
        if (Status == "Posted")
            throw new BadRequestException("Journal entry is already posted");
        if (IsReversed)
            throw new BadRequestException("Cannot post a reversed journal entry");
        if (TotalDebit != TotalCredit)
            throw new BadRequestException("Journal entry is not balanced. Debits must equal credits");
        
        Status = "Posted";
        PostedDate = postedDate ?? DateTime.UtcNow;
        PostedBy = postedBy;
    }
    
    public void Approve(Guid approvedBy, DateTime? approvedDate = null)
    {
        if (Status != "Posted")
            throw new BadRequestException("Journal entry must be posted before approval");
        if (IsReversed)
            throw new BadRequestException("Cannot approve a reversed journal entry");
        
        Status = "Approved";
        ApprovedDate = approvedDate ?? DateTime.UtcNow;
        ApprovedBy = approvedBy;
    }
    
    public void Void()
    {
        if (Status == "Approved")
            throw new BadRequestException("Cannot void an approved journal entry. Must be reversed instead");
        
        Status = "Voided";
        IsActive = false;
    }
    
    public void Reverse(Guid reversedEntryId, DateTime? reversedDate = null)
    {
        if (Status != "Posted" && Status != "Approved")
            throw new BadRequestException("Only posted or approved entries can be reversed");
        if (IsReversed)
            throw new BadRequestException("Journal entry is already reversed");
        
        IsReversed = true;
        ReversedEntryId = reversedEntryId;
        ReversedDate = reversedDate ?? DateTime.UtcNow;
    }
    
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    
    public bool IsBalanced() => TotalDebit == TotalCredit;
    
    private static bool IsValidEntryType(string entryType)
    {
        var validTypes = new[] { "Standard", "Adjusting", "Closing", "Reversing" };
        return validTypes.Contains(entryType, StringComparer.OrdinalIgnoreCase);
    }
}
