namespace FSH.Module.Accounting.Domain;

public class BankReconciliationLine : AuditableEntity<Guid>, IMustHaveTenant
{
    public Guid BankReconciliationId { get; private set; }
    public Guid TransactionId { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }
    public bool IsCleared { get; private set; }
    public string TenantId { get; private set; } = default!;

    private BankReconciliationLine() { }

    public static BankReconciliationLine Create(Guid bankReconciliationId, Guid transactionId, DateTime transactionDate, decimal amount, string? description, string tenantId, Guid createdBy, string createdByUserName)
    {
        if (bankReconciliationId == Guid.Empty) throw new ArgumentException("bankReconciliationId is required", nameof(bankReconciliationId));
        if (amount == 0) throw new ArgumentException("amount cannot be zero", nameof(amount));

        return new BankReconciliationLine
        {
            Id = Guid.NewGuid(),
            BankReconciliationId = bankReconciliationId,
            TransactionId = transactionId,
            TransactionDate = transactionDate.Date,
            Amount = amount,
            Description = description?.Trim(),
            IsCleared = false,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }
}