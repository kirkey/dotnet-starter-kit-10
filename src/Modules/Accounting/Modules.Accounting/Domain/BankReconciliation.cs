namespace FSH.Modules.Accounting.Domain;

/// <summary>
/// Represents a BankReconciliation in the accounting system.
/// </summary>
public class BankReconciliation : AuditableEntity<Guid>, IMustHaveTenant
{
    public string ReconciliationNumber { get; private set; } = default!;
    public Guid BankAccountId { get; private set; }
    public DateTime StatementDate { get; private set; }
    public decimal StatementBalance { get; private set; }
    public decimal BookBalance { get; private set; }
    public decimal Difference { get; private set; }

    public string Status { get; private set; } = "Draft"; // Draft, InProgress, Reconciled
    public DateTime? ReconciledDate { get; private set; }
    public Guid? ReconciledBy { get; private set; }

    public decimal AdjustmentAmount { get; private set; }
    public string? AdjustmentNotes { get; private set; }

    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;

    // Navigation
    public ICollection<BankReconciliationLine>? Lines { get; private set; }

    private BankReconciliation() { }

    public static BankReconciliation Create(
        string reconciliationNumber,
        Guid bankAccountId,
        DateTime statementDate,
        decimal statementBalance,
        decimal bookBalance,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(reconciliationNumber))
            throw new BadRequestException("Reconciliation number is required");
        if (bankAccountId == Guid.Empty)
            throw new BadRequestException("Bank account is required");
        if (statementBalance < 0)
            throw new BadRequestException("Statement balance cannot be negative");

        return new BankReconciliation
        {
            Id = Guid.NewGuid(),
            ReconciliationNumber = reconciliationNumber.Trim(),
            BankAccountId = bankAccountId,
            StatementDate = statementDate.Date,
            StatementBalance = statementBalance,
            BookBalance = bookBalance,
            Difference = statementBalance - bookBalance,
            Status = "Draft",
            AdjustmentAmount = 0,
            AdjustmentNotes = null,
            Description = description?.Trim(),
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }

    public void Update(
        DateTime statementDate,
        decimal statementBalance,
        decimal bookBalance,
        string? description = null)
    {
        if (Status == "Reconciled")
            throw new BadRequestException("Cannot update a reconciled statement");

        StatementDate = statementDate.Date;
        StatementBalance = statementBalance;
        BookBalance = bookBalance;
        Difference = StatementBalance - BookBalance;
        Description = description?.Trim();
    }

    public void AddAdjustment(decimal amount, string? notes = null)
    {
        if (amount == 0) throw new BadRequestException("Adjustment amount cannot be zero");
        AdjustmentAmount += amount;
        AdjustmentNotes = notes?.Trim();
        Difference = (StatementBalance + AdjustmentAmount) - BookBalance;
    }

    public void MarkInProgress()
    {
        if (Status != "Draft")
            throw new BadRequestException("Only draft reconciliations can be moved to in-progress");
        Status = "InProgress";
    }

    public void Reconcile(Guid reconciledBy, DateTime? reconciledDate = null)
    {
        if (Status != "InProgress")
            throw new BadRequestException("Reconciliation must be in progress to be completed");
        if (Difference != 0)
            throw new BadRequestException("Cannot reconcile when difference is not zero. Apply adjustments first.");

        Status = "Reconciled";
        ReconciledDate = reconciledDate ?? DateTime.UtcNow;
        ReconciledBy = reconciledBy;
        IsActive = false; // reconciled items become read-only
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
