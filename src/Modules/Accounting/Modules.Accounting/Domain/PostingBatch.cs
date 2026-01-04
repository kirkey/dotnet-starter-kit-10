namespace FSH.Modules.Accounting.Domain;

/// <summary>
/// Represents a PostingBatch in the accounting system with lifecycle operations.
/// </summary>
public class PostingBatch : AuditableEntity<Guid>, IMustHaveTenant
{
    // Human friendly identifier for the batch
    public string Name { get; private set; } = default!;

    // Effective date for the batch
    public DateTime BatchDate { get; private set; }

    public string? Description { get; private set; }

    // Workflow status: Draft, Pending, Approved, Posted, Reversed, Rejected
    public string Status { get; private set; } = "Draft";

    public decimal TotalDebits { get; private set; }
    public decimal TotalCredits { get; private set; }
    public int EntryCount { get; private set; }

    public DateTime? PostedOn { get; private set; }
    public Guid? PostedBy { get; private set; }

    public DateTime? ReversedOn { get; private set; }
    public Guid? ReversedBy { get; private set; }

    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;

    private PostingBatch() { }

    public static PostingBatch Create(
        string name,
        DateTime batchDate,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException("Batch name is required");

        return new PostingBatch
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            BatchDate = batchDate.Date,
            Description = description?.Trim(),
            Status = "Draft",
            TotalDebits = 0,
            TotalCredits = 0,
            EntryCount = 0,
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }

    public void Update(DateTime batchDate, string? description = null)
    {
        if (Status != "Draft" && Status != "Pending")
            throw new BadRequestException("Only Draft or Pending batches can be updated");

        BatchDate = batchDate.Date;
        Description = description?.Trim();
    }

    public void Approve(Guid approverId)
    {
        if (Status == "Approved")
            throw new BadRequestException("Posting batch already approved");

        if (Status != "Draft" && Status != "Pending")
            throw new BadRequestException("Only Draft or Pending batches can be approved");

        Status = "Approved";
        ApprovedBy = approverId;
        ApprovedOn = DateTime.UtcNow;
    }

    public void Reject(Guid rejectedBy)
    {
        if (Status == "Rejected")
            throw new BadRequestException("Posting batch already rejected");

        Status = "Rejected";
        ApprovedBy = rejectedBy;
        ApprovedOn = DateTime.UtcNow;
    }

    public void Post(Guid postedBy)
    {
        if (Status != "Approved")
            throw new BadRequestException("Only approved batches can be posted");

        // Totals may be calculated from related entries by higher level logic; keep simple guard here
        if (TotalDebits != TotalCredits)
            throw new BadRequestException("Posting batch is not balanced. Debits must equal credits");

        Status = "Posted";
        PostedBy = postedBy;
        PostedOn = DateTime.UtcNow;
    }

    public void Reverse(Guid reversedBy)
    {
        if (Status != "Posted")
            throw new BadRequestException("Only posted batches can be reversed");

        Status = "Reversed";
        ReversedBy = reversedBy;
        ReversedOn = DateTime.UtcNow;
    }

    public void UpdateTotals(decimal totalDebits, decimal totalCredits, int entryCount)
    {
        TotalDebits = totalDebits;
        TotalCredits = totalCredits;
        EntryCount = entryCount;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
