using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a Journal Entry Line in the accounting system.
/// Each line represents a debit or credit to a specific account.
/// </summary>
public class JournalEntryLine : AuditableEntity<Guid>, IMustHaveTenant
{
    // Parent Reference
    public Guid JournalEntryId { get; private set; }
    
    // Line Identification
    public int LineNumber { get; private set; }
    
    // Account Reference
    public Guid AccountId { get; private set; }
    public string AccountCode { get; private set; } = default!;
    public string AccountName { get; private set; } = default!;
    
    // Financial Properties
    public decimal Debit { get; private set; }
    public decimal Credit { get; private set; }
    public decimal Amount { get; private set; } // Absolute value (Debit or Credit)
    public string TransactionType { get; private set; } = default!; // "Debit" or "Credit"
    
    // Reference & Documentation
    public string? ReferenceNumber { get; private set; }
    public string? Description { get; private set; }
    public string? Notes { get; private set; }
    
    // Cost Allocation (Optional)
    public Guid? CostCenterId { get; private set; }
    public Guid? DepartmentId { get; private set; }
    public Guid? ProjectId { get; private set; }
    
    // Audit & Status
    public bool IsActive { get; private set; } = true;
    
    // Navigation Properties (not mapped by default)
    public JournalEntry? JournalEntry { get; private set; }
    public ChartOfAccount? Account { get; private set; }
    
    private JournalEntryLine() { }
    
    public static JournalEntryLine Create(
        Guid journalEntryId,
        int lineNumber,
        Guid accountId,
        string accountCode,
        string accountName,
        decimal amount,
        string transactionType,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? referenceNumber = null,
        string? description = null,
        string? notes = null,
        Guid? costCenterId = null,
        Guid? departmentId = null,
        Guid? projectId = null)
    {
        // Validation
        if (journalEntryId == Guid.Empty)
            throw new BadRequestException("Journal entry ID is required");
        if (accountId == Guid.Empty)
            throw new BadRequestException("Account ID is required");
        if (string.IsNullOrWhiteSpace(accountCode))
            throw new BadRequestException("Account code is required");
        if (string.IsNullOrWhiteSpace(accountName))
            throw new BadRequestException("Account name is required");
        if (amount < 0)
            throw new BadRequestException("Amount must be positive");
        if (!IsValidTransactionType(transactionType))
            throw new BadRequestException("Transaction type must be Debit or Credit");
        if (lineNumber <= 0)
            throw new BadRequestException("Line number must be positive");
        
        return new JournalEntryLine
        {
            Id = Guid.NewGuid(),
            JournalEntryId = journalEntryId,
            LineNumber = lineNumber,
            AccountId = accountId,
            AccountCode = accountCode.Trim(),
            AccountName = accountName.Trim(),
            Amount = amount,
            TransactionType = transactionType.Trim(),
            Debit = transactionType.Equals("Debit", StringComparison.OrdinalIgnoreCase) ? amount : 0,
            Credit = transactionType.Equals("Credit", StringComparison.OrdinalIgnoreCase) ? amount : 0,
            ReferenceNumber = referenceNumber?.Trim(),
            Description = description?.Trim(),
            Notes = notes?.Trim(),
            CostCenterId = costCenterId,
            DepartmentId = departmentId,
            ProjectId = projectId,
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }
    
    public void Update(
        int lineNumber,
        Guid accountId,
        string accountCode,
        string accountName,
        decimal amount,
        string transactionType,
        string? referenceNumber = null,
        string? description = null,
        string? notes = null,
        Guid? costCenterId = null,
        Guid? departmentId = null,
        Guid? projectId = null)
    {
        if (accountId == Guid.Empty)
            throw new BadRequestException("Account ID is required");
        if (amount < 0)
            throw new BadRequestException("Amount must be positive");
        if (!IsValidTransactionType(transactionType))
            throw new BadRequestException("Transaction type must be Debit or Credit");
        if (lineNumber <= 0)
            throw new BadRequestException("Line number must be positive");
        
        LineNumber = lineNumber;
        AccountId = accountId;
        AccountCode = accountCode.Trim();
        AccountName = accountName.Trim();
        Amount = amount;
        TransactionType = transactionType.Trim();
        Debit = transactionType.Equals("Debit", StringComparison.OrdinalIgnoreCase) ? amount : 0;
        Credit = transactionType.Equals("Credit", StringComparison.OrdinalIgnoreCase) ? amount : 0;
        ReferenceNumber = referenceNumber?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();
        CostCenterId = costCenterId;
        DepartmentId = departmentId;
        ProjectId = projectId;
    }
    
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    
    private static bool IsValidTransactionType(string transactionType)
    {
        return transactionType.Equals("Debit", StringComparison.OrdinalIgnoreCase) ||
               transactionType.Equals("Credit", StringComparison.OrdinalIgnoreCase);
    }
}
