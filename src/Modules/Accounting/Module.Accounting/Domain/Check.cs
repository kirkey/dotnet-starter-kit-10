using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a Check in the accounting system.
/// </summary>
public class Check : AuditableEntity<Guid>, IMustHaveTenant
{
    public string CheckNumber { get; private set; } = default!;
    public DateTime CheckDate { get; private set; }
    public string CheckType { get; private set; } = default!; // e.g., Manual, Preprinted

    // Payee
    public Guid? PayeeId { get; private set; }
    public string PayeeName { get; private set; } = default!;

    // Bank info
    public Guid BankAccountId { get; private set; }
    public string AccountNumber { get; private set; } = default!;

    // Financial
    public decimal Amount { get; private set; }

    // Status
    public string Status { get; private set; } = "Draft"; // Draft, Issued, Printed, Cleared, Void, StopPayment
    public DateTime? PrintedDate { get; private set; }
    public DateTime? ClearedDate { get; private set; }
    public Guid? ClearedBy { get; private set; }

    // Related
    public Guid? JournalEntryId { get; private set; }

    // Reference & Notes
    public string? ReferenceNumber { get; private set; }
    public string? Notes { get; private set; }

    // Audit & Status
    public bool IsActive { get; private set; } = true;

    private Check() { }

    public static Check Create(
        string checkNumber,
        DateTime checkDate,
        string checkType,
        Guid bankAccountId,
        string accountNumber,
        decimal amount,
        string payeeName,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        Guid? payeeId = null,
        string? referenceNumber = null,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(checkNumber))
            throw new BadRequestException("Check number is required");
        if (bankAccountId == Guid.Empty)
            throw new BadRequestException("Bank account is required");
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new BadRequestException("Account number is required");
        if (amount <= 0)
            throw new BadRequestException("Amount must be greater than zero");
        if (string.IsNullOrWhiteSpace(payeeName))
            throw new BadRequestException("Payee name is required");

        return new Check
        {
            Id = Guid.NewGuid(),
            CheckNumber = checkNumber.Trim(),
            CheckDate = checkDate.Date,
            CheckType = checkType.Trim(),
            PayeeId = payeeId,
            PayeeName = payeeName.Trim(),
            BankAccountId = bankAccountId,
            AccountNumber = accountNumber.Trim(),
            Amount = amount,
            Status = "Draft",
            ReferenceNumber = referenceNumber?.Trim(),
            Notes = notes?.Trim(),
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }

    public void Update(
        string checkNumber,
        DateTime checkDate,
        string checkType,
        Guid bankAccountId,
        string accountNumber,
        decimal amount,
        string payeeName,
        Guid? payeeId = null,
        string? referenceNumber = null,
        string? notes = null)
    {
        if (Status == "Cleared")
            throw new BadRequestException("Cannot update a cleared check");
        if (Status == "Void")
            throw new BadRequestException("Cannot update a voided check");

        if (string.IsNullOrWhiteSpace(checkNumber))
            throw new BadRequestException("Check number is required");
        if (bankAccountId == Guid.Empty)
            throw new BadRequestException("Bank account is required");
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new BadRequestException("Account number is required");
        if (amount <= 0)
            throw new BadRequestException("Amount must be greater than zero");
        if (string.IsNullOrWhiteSpace(payeeName))
            throw new BadRequestException("Payee name is required");

        CheckNumber = checkNumber.Trim();
        CheckDate = checkDate.Date;
        CheckType = checkType.Trim();
        BankAccountId = bankAccountId;
        AccountNumber = accountNumber.Trim();
        Amount = amount;
        PayeeName = payeeName.Trim();
        PayeeId = payeeId;
        ReferenceNumber = referenceNumber?.Trim();
        Notes = notes?.Trim();
    }

    public void Issue()
    {
        if (Status != "Draft")
            throw new BadRequestException("Only draft checks can be issued");
        Status = "Issued";
    }

    public void Print()
    {
        if (Status == "Void")
            throw new BadRequestException("Cannot print a voided check");
        if (Status == "Cleared")
            throw new BadRequestException("Cannot print a cleared check");
        Status = "Printed";
        PrintedDate = DateTime.UtcNow;
    }

    public void Clear(Guid clearedBy, DateTime? clearedDate = null, Guid? journalEntryId = null)
    {
        if (Status == "Void")
            throw new BadRequestException("Cannot clear a voided check");
        if (Status == "StopPayment")
            throw new BadRequestException("Cannot clear a check with stop payment");
        if (Status == "Cleared")
            throw new BadRequestException("Check is already cleared");

        Status = "Cleared";
        ClearedDate = clearedDate ?? DateTime.UtcNow;
        ClearedBy = clearedBy;
        JournalEntryId = journalEntryId;
        IsActive = false;
    }

    public void Void()
    {
        if (Status == "Cleared")
            throw new BadRequestException("Cannot void a cleared check");
        Status = "Void";
        IsActive = false;
    }

    public void StopPayment()
    {
        if (Status == "Cleared")
            throw new BadRequestException("Cannot stop payment on a cleared check");
        Status = "StopPayment";
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}

