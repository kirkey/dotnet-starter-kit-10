using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Domain;

/// <summary>
/// Represents a single account in the chart of accounts, including type, hierarchy, and regulatory classification.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define USOA-compliant account structure for utility accounting.
/// - Support hierarchical account organization with parent-child relationships.
/// - Enable regulatory reporting and FERC compliance tracking.
/// - Control posting permissions with control account designations.
/// - Maintain account balances and trial balance calculations.
/// </remarks>
public class ChartOfAccount : AuditableEntity<Guid>, IMustHaveTenant
{
    /// <summary>
    /// The unique account code (e.g., USOA 101, 403). Trimmed and length-limited.
    /// Example: "101" for Cash, "403" for Overhead Line Expenses.
    /// </summary>
    public string AccountCode { get; private set; } = default!;

    /// <summary>
    /// The official account name. Also stored in base Name for compatibility.
    /// Example: "Cash and Cash Equivalents", "Overhead Line Expenses".
    /// </summary>
    public string AccountName { get; private set; } = default!;

    /// <summary>
    /// Legacy name field for compatibility. Mirrors AccountName.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// The account type: Asset, Liability, Equity, Revenue, or Expense.
    /// Example: "Asset" for cash accounts, "Revenue" for sales accounts.
    /// </summary>
    public string AccountType { get; private set; } = default!;

    /// <summary>
    /// Optional parent account identifier for hierarchical structures.
    /// Example: links sub-accounts to their parent control accounts.
    /// </summary>
    public Guid? ParentAccountId { get; private set; }

    /// <summary>
    /// The USOA (Uniform System of Accounts) category.
    /// Example: "Production", "Transmission", "Distribution".
    /// </summary>
    public string UsoaCategory { get; private set; } = default!;

    /// <summary>
    /// Whether the account is active and can be used in postings. Default: true.
    /// Used to retire accounts without deleting historical data.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Optional dotted parent code path for hierarchy representation.
    /// Example: "100.101" for cash under current assets.
    /// </summary>
    public string ParentCode { get; private set; } = string.Empty;

    /// <summary>
    /// Current balance of the account. Default: 0.00. Updated via UpdateBalance method.
    /// Example: 150000.00 for cash account balance.
    /// </summary>
    public decimal Balance { get; private set; }

    /// <summary>
    /// Whether the account is a control account (summary) that disallows direct postings.
    /// Default: false. Control accounts roll up sub-account balances.
    /// </summary>
    public bool IsControlAccount { get; private set; }

    /// <summary>
    /// Normal balance side, e.g., "Debit" or "Credit". Used for trial balance interpretation.
    /// Example: "Debit" for assets/expenses, "Credit" for liabilities/revenues.
    /// </summary>
    public string NormalBalance { get; private set; } = "Debit";

    /// <summary>
    /// Derived hierarchical depth computed from ParentCode.
    /// Example: 0 for top-level, 1 for first sub-level, etc.
    /// </summary>
    public int AccountLevel { get; private set; }

    /// <summary>
    /// Whether direct postings are allowed. Set to !IsControlAccount by convention.
    /// Default: true for detail accounts, false for control accounts.
    /// </summary>
    public bool AllowDirectPosting { get; private set; } = true;

    /// <summary>
    /// Indicates if this account conforms to USOA standards.
    /// Default: true for regulatory compliance. Example: FERC account classifications.
    /// </summary>
    public bool IsUsoaCompliant { get; private set; } = true;

    /// <summary>
    /// Optional regulatory classification, e.g., FERC category text.
    /// Example: "Electric Plant in Service", "Operating Revenues".
    /// </summary>
    public string? RegulatoryClassification { get; private set; }

    /// <summary>
    /// Optional description of the account's purpose and usage.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Optional notes for additional information.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Tenant identifier for multi-tenancy isolation.
    /// </summary>
    public string TenantId { get; private set; } = default!;

    // Parameterless constructor for EF Core
    private ChartOfAccount() { }

    private ChartOfAccount(
        string accountCode,
        string accountName,
        string accountType,
        string usoaCategory,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        Guid? parentAccountId = null,
        string? parentCode = null,
        decimal balance = 0,
        bool isControlAccount = false,
        string normalBalance = "Debit",
        bool isUsoaCompliant = true,
        string? regulatoryClassification = null,
        string? description = null,
        string? notes = null)
    {
        var ac = (accountCode ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(ac))
            throw new BadRequestException("Account code cannot be empty");
        if (ac.Length > AccountingStringLengths.AccountCode)
            throw new BadRequestException($"Account code cannot exceed {AccountingStringLengths.AccountCode} characters.");

        var an = (accountName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(an))
            throw new BadRequestException("Account name cannot be empty");
        if (an.Length > AccountingStringLengths.AccountName)
            throw new BadRequestException($"Account name cannot exceed {AccountingStringLengths.AccountName} characters.");

        var at = (accountType ?? string.Empty).Trim();
        if (!IsValidAccountType(at))
            throw new BadRequestException($"Invalid account type: {accountType}. Must be Asset, Liability, Equity, Revenue, or Expense.");
        if (at.Length > AccountingStringLengths.AccountType)
            throw new BadRequestException($"Account type cannot exceed {AccountingStringLengths.AccountType} characters.");

        var uc = NormalizeUsoaCategory((usoaCategory ?? string.Empty).Trim());
        if (!IsValidUsoaCategory(uc))
            throw new BadRequestException($"Invalid USOA category: {usoaCategory}");
        if (uc.Length > AccountingStringLengths.UsoaCategory)
            throw new BadRequestException($"USOA category cannot exceed {AccountingStringLengths.UsoaCategory} characters.");

        Id = Guid.NewGuid();
        AccountCode = ac;
        AccountName = an;
        Name = an; // Keep for compatibility
        AccountType = at;
        ParentAccountId = parentAccountId;
        UsoaCategory = uc;
        IsActive = true;
        ParentCode = (parentCode ?? string.Empty).Trim();
        if (ParentCode.Length > AccountingStringLengths.Regular)
            ParentCode = ParentCode[..AccountingStringLengths.Regular];

        Balance = balance;
        IsControlAccount = isControlAccount;
        NormalBalance = (normalBalance ?? "Debit").Trim();
        if (NormalBalance.Length > AccountingStringLengths.NormalBalance)
            NormalBalance = NormalBalance[..AccountingStringLengths.NormalBalance];

        AccountLevel = CalculateAccountLevel(ParentCode);
        AllowDirectPosting = !isControlAccount;
        IsUsoaCompliant = isUsoaCompliant;
        RegulatoryClassification = regulatoryClassification?.Trim();
        if (RegulatoryClassification?.Length > AccountingStringLengths.RegulatoryClassification)
            RegulatoryClassification = RegulatoryClassification[..AccountingStringLengths.RegulatoryClassification];

        Description = description?.Trim();
        if (Description?.Length > AccountingStringLengths.Description)
            Description = Description[..AccountingStringLengths.Description];

        Notes = notes?.Trim();
        if (Notes?.Length > AccountingStringLengths.Notes)
            Notes = Notes[..AccountingStringLengths.Notes];

        TenantId = tenantId;
        CreatedBy = createdBy;
        CreatedByUserName = createdByUserName;
        CreatedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Factory method to create a new chart of account with validation for code, name, type, and category.
    /// </summary>
    public static ChartOfAccount Create(
        string accountCode,
        string accountName,
        string accountType,
        string usoaCategory,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        Guid? parentAccountId = null,
        string? parentCode = null,
        decimal balance = 0,
        bool isControlAccount = false,
        string normalBalance = "Debit",
        bool isUsoaCompliant = true,
        string? regulatoryClassification = null,
        string? description = null,
        string? notes = null)
    {
        return new ChartOfAccount(
            accountCode, accountName, accountType, usoaCategory, tenantId, createdBy, createdByUserName,
            parentAccountId, parentCode, balance, isControlAccount, normalBalance, isUsoaCompliant,
            regulatoryClassification, description, notes);
    }

    /// <summary>
    /// Update account metadata while keeping invariants. Trims and length-limits inputs.
    /// </summary>
    public void Update(
        string? accountName = null,
        string? accountType = null,
        string? usoaCategory = null,
        Guid? parentAccountId = null,
        string? parentCode = null,
        bool? isControlAccount = null,
        string? normalBalance = null,
        bool? isUsoaCompliant = null,
        string? regulatoryClassification = null,
        string? description = null,
        string? notes = null)
    {
        if (!string.IsNullOrWhiteSpace(accountName) && AccountName != accountName.Trim())
        {
            var an = accountName.Trim();
            if (an.Length > AccountingStringLengths.AccountName)
                throw new BadRequestException($"Account name cannot exceed {AccountingStringLengths.AccountName} characters.");
            Name = an;
            AccountName = an;
        }

        if (!string.IsNullOrWhiteSpace(accountType) && AccountType != accountType.Trim())
        {
            var at = accountType.Trim();
            if (!IsValidAccountType(at))
                throw new BadRequestException($"Invalid account type: {accountType}");
            if (at.Length > AccountingStringLengths.AccountType)
                throw new BadRequestException($"Account type cannot exceed {AccountingStringLengths.AccountType} characters.");
            AccountType = at;
        }

        if (!string.IsNullOrWhiteSpace(usoaCategory) && UsoaCategory != usoaCategory.Trim())
        {
            var uc = NormalizeUsoaCategory(usoaCategory.Trim());
            if (!IsValidUsoaCategory(uc))
                throw new BadRequestException($"Invalid USOA category: {usoaCategory}");
            if (uc.Length > AccountingStringLengths.UsoaCategory)
                throw new BadRequestException($"USOA category cannot exceed {AccountingStringLengths.UsoaCategory} characters.");
            UsoaCategory = uc;
        }

        if (parentAccountId.HasValue && parentAccountId != ParentAccountId)
        {
            ParentAccountId = parentAccountId;
        }

        if (!string.IsNullOrWhiteSpace(parentCode) && ParentCode != parentCode.Trim())
        {
            var pc = parentCode.Trim();
            if (pc.Length > AccountingStringLengths.Regular)
                pc = pc[..AccountingStringLengths.Regular];
            ParentCode = pc;
            AccountLevel = CalculateAccountLevel(ParentCode);
        }

        if (isControlAccount.HasValue && IsControlAccount != isControlAccount.Value)
        {
            IsControlAccount = isControlAccount.Value;
            AllowDirectPosting = !IsControlAccount;
        }

        if (!string.IsNullOrWhiteSpace(normalBalance) && NormalBalance != normalBalance.Trim())
        {
            var nb = normalBalance.Trim();
            if (nb.Length > AccountingStringLengths.NormalBalance)
                nb = nb[..AccountingStringLengths.NormalBalance];
            NormalBalance = nb;
        }

        if (isUsoaCompliant.HasValue && IsUsoaCompliant != isUsoaCompliant.Value)
        {
            IsUsoaCompliant = isUsoaCompliant.Value;
        }

        if (!string.IsNullOrWhiteSpace(regulatoryClassification) && RegulatoryClassification != regulatoryClassification.Trim())
        {
            var rc = regulatoryClassification.Trim();
            if (rc.Length > AccountingStringLengths.RegulatoryClassification)
                rc = rc[..AccountingStringLengths.RegulatoryClassification];
            RegulatoryClassification = rc;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != description?.Trim())
        {
            var d = description.Trim();
            if (d.Length > AccountingStringLengths.Description)
                d = d[..AccountingStringLengths.Description];
            Description = d;
        }

        if (!string.IsNullOrWhiteSpace(notes) && Notes != notes?.Trim())
        {
            var n = notes.Trim();
            if (n.Length > AccountingStringLengths.Notes)
                n = n[..AccountingStringLengths.Notes];
            Notes = n;
        }
    }

    /// <summary>
    /// Update the current balance of the account.
    /// </summary>
    public void UpdateBalance(decimal newBalance)
    {
        Balance = newBalance;
    }

    /// <summary>
    /// Activate a previously deactivated account.
    /// </summary>
    public void Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
        }
    }

    /// <summary>
    /// Deactivate the account to prevent use in postings.
    /// </summary>
    public void Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
        }
    }

    private static bool IsValidAccountType(string accountType)
    {
        var validTypes = new[] { "Asset", "Liability", "Equity", "Revenue", "Expense" };
        return validTypes.Contains(accountType.Trim(), StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsValidUsoaCategory(string usoaCategory)
    {
        var validCategories = new[]
        {
            "Production", "Transmission", "Distribution", "Customer Accounts",
            "Customer Service", "Sales", "Administrative", "General",
            "Maintenance", "Operation", "Operations", "COGS", "Inventory"
        };
        return validCategories.Contains(usoaCategory.Trim(), StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Normalize common USOA category synonyms to canonical short values.
    /// Examples: "Cost of Goods Sold" -> "COGS".
    /// </summary>
    private static string NormalizeUsoaCategory(string usoaCategory)
    {
        if (string.IsNullOrWhiteSpace(usoaCategory))
            return string.Empty;

        var x = usoaCategory.Trim();
        return x.ToUpperInvariant() switch
        {
            "COST OF GOODS SOLD" => "COGS",
            "COST-OF-GOODS-SOLD" => "COGS",
            "COSTS OF GOODS SOLD" => "COGS",
            "COGS" => "COGS",
            "INVENTORY" => "Inventory",
            "OPERATIONS" => "Operations",
            _ => x
        };
    }

    private static int CalculateAccountLevel(string? parentCode)
    {
        if (string.IsNullOrWhiteSpace(parentCode))
            return 1;

        try
        {
            return parentCode.Split('.').Length + 1;
        }
        catch
        {
            return 1;
        }
    }
}
