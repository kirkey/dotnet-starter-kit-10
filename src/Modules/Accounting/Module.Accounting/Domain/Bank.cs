namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a Bank in the accounting system.
/// </summary>
public class Bank : AuditableEntity<Guid>, IMustHaveTenant
{
    public string BankName { get; private set; } = default!;
    public string? BankCode { get; private set; }
    public string? Address { get; private set; }
    public string? ContactName { get; private set; }
    public string? ContactPhone { get; private set; }
    public string? RoutingNumber { get; private set; }
    public string? SwiftCode { get; private set; }
    public string? CurrencyCode { get; private set; }

    // Balances
    public decimal OpeningBalance { get; private set; }
    public decimal CurrentBalance { get; private set; }

    public bool IsDefault { get; private set; }

    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;

    private Bank() { }

    public static Bank Create(
        string bankName,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? bankCode = null,
        string? address = null,
        string? contactName = null,
        string? contactPhone = null,
        string? routingNumber = null,
        string? swiftCode = null,
        string? currencyCode = null,
        decimal openingBalance = 0,
        bool isDefault = false,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(bankName))
            throw new BadRequestException("Bank name is required");
        if (openingBalance < 0)
            throw new BadRequestException("Opening balance cannot be negative");

        return new Bank
        {
            Id = Guid.NewGuid(),
            BankName = bankName.Trim(),
            BankCode = bankCode?.Trim(),
            Address = address?.Trim(),
            ContactName = contactName?.Trim(),
            ContactPhone = contactPhone?.Trim(),
            RoutingNumber = routingNumber?.Trim(),
            SwiftCode = swiftCode?.Trim(),
            CurrencyCode = currencyCode?.Trim(),
            OpeningBalance = openingBalance,
            CurrentBalance = openingBalance,
            IsDefault = isDefault,
            Description = description?.Trim(),
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }

    public void Update(
        string bankName,
        string? bankCode = null,
        string? address = null,
        string? contactName = null,
        string? contactPhone = null,
        string? routingNumber = null,
        string? swiftCode = null,
        string? currencyCode = null,
        decimal? openingBalance = null,
        bool? isDefault = null,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(bankName))
            throw new BadRequestException("Bank name is required");
        if (openingBalance.HasValue && openingBalance.Value < 0)
            throw new BadRequestException("Opening balance cannot be negative");

        BankName = bankName.Trim();
        BankCode = bankCode?.Trim();
        Address = address?.Trim();
        ContactName = contactName?.Trim();
        ContactPhone = contactPhone?.Trim();
        RoutingNumber = routingNumber?.Trim();
        SwiftCode = swiftCode?.Trim();
        CurrencyCode = currencyCode?.Trim();

        if (openingBalance.HasValue)
        {
            // adjust current balance relative to change in opening balance
            var diff = openingBalance.Value - OpeningBalance;
            OpeningBalance = openingBalance.Value;
            CurrentBalance += diff;
        }

        if (isDefault.HasValue) IsDefault = isDefault.Value;
        Description = description?.Trim();
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new BadRequestException("Amount must be positive");
        CurrentBalance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0) throw new BadRequestException("Amount must be positive");
        CurrentBalance -= amount;
    }

    public void SetAsDefault()
    {
        IsDefault = true;
    }

    public void UnsetDefault()
    {
        IsDefault = false;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
