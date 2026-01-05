using FluentValidation;

namespace FSH.Module.Accounting.Features;

/// <summary>
/// Centralized validation rules for Accounting module using constants.
/// 
/// **Purpose:**
/// Provides reusable FluentValidation extension methods for common Accounting properties.
/// All rules use constants from AccountingStringLengths to ensure consistency across validators.
/// 
/// **Benefits:**
/// - Single source of truth for validation rules
/// - DRY principle compliance
/// - Easy to maintain and update
/// - Fluent API for clean validator definitions
/// - Automatic consistency with database constraints
/// 
/// **Usage Pattern:**
/// RuleFor(x => x.AccountCode).ValidateAccountCode();
/// RuleFor(x => x.AccountName).ValidateAccountName();
/// RuleFor(x => x.Description).ValidateDescription();
/// 
/// **Naming Convention:**
/// Validate{Property}() - e.g., ValidateAccountCode, ValidateAccountName
/// </summary>
public static class AccountingValidationExtensions
{
    // ========== Common Property Validations ==========

    /// <summary>
    /// Applies validation rules for account code property.
    /// 
    /// Rules:
    /// - Must not be empty
    /// - Must not exceed AccountingStringLengths.AccountCode (16 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.AccountCode).ValidateAccountCode();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string> ValidateAccountCode<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Account code is required")
            .MaximumLength(AccountingStringLengths.AccountCode)
            .WithMessage($"Account code must not exceed {AccountingStringLengths.AccountCode} characters");
    }

    /// <summary>
    /// Applies validation rules for account name property.
    /// 
    /// Rules:
    /// - Must not be empty
    /// - Must not exceed AccountingStringLengths.AccountName (128 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.AccountName).ValidateAccountName();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string> ValidateAccountName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Account name is required")
            .MaximumLength(AccountingStringLengths.AccountName)
            .WithMessage($"Account name must not exceed {AccountingStringLengths.AccountName} characters");
    }

    /// <summary>
    /// Applies validation rules for description property.
    /// 
    /// Rules:
    /// - Optional field
    /// - Must not exceed AccountingStringLengths.Huge (512 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.Description).ValidateDescription();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string?> ValidateDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
        where T : class
    {
        return ruleBuilder
            .MaximumLength(AccountingStringLengths.Huge)
            .WithMessage($"Description must not exceed {AccountingStringLengths.Huge} characters")
            .When(x => x != null);
    }

    /// <summary>
    /// Applies validation rules for notes property.
    /// 
    /// Rules:
    /// - Optional field
    /// - Must not exceed AccountingStringLengths.XXHuge (2048 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.Notes).ValidateNotes();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string?> ValidateNotes<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
        where T : class
    {
        return ruleBuilder
            .MaximumLength(AccountingStringLengths.XXHuge)
            .WithMessage($"Notes must not exceed {AccountingStringLengths.XXHuge} characters")
            .When(x => x != null);
    }

    // ========== Journal Entry Validations ==========

    /// <summary>
    /// Applies validation rules for journal entry number property.
    /// 
    /// Rules:
    /// - Must not be empty
    /// - Must not exceed AccountingStringLengths.JournalEntryNumber (64 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.JournalEntryNumber).ValidateJournalEntryNumber();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string> ValidateJournalEntryNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Journal entry number is required")
            .MaximumLength(AccountingStringLengths.JournalEntryNumber)
            .WithMessage($"Journal entry number must not exceed {AccountingStringLengths.JournalEntryNumber} characters");
    }

    // ========== Invoice Validations ==========

    /// <summary>
    /// Applies validation rules for invoice number property.
    /// 
    /// Rules:
    /// - Must not be empty
    /// - Must not exceed AccountingStringLengths.InvoiceNumber (64 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.InvoiceNumber).ValidateInvoiceNumber();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string> ValidateInvoiceNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Invoice number is required")
            .MaximumLength(AccountingStringLengths.InvoiceNumber)
            .WithMessage($"Invoice number must not exceed {AccountingStringLengths.InvoiceNumber} characters");
    }

    // ========== Customer/Vendor Validations ==========

    /// <summary>
    /// Applies validation rules for customer/vendor name property.
    /// 
    /// Rules:
    /// - Must not be empty
    /// - Must not exceed AccountingStringLengths.XLarge (128 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.CustomerName).ValidateCustomerName();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string> ValidateCustomerName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(AccountingStringLengths.XLarge)
            .WithMessage($"Customer name must not exceed {AccountingStringLengths.XLarge} characters");
    }

    /// <summary>
    /// Applies validation rules for vendor name property.
    /// 
    /// Rules:
    /// - Must not be empty
    /// - Must not exceed AccountingStringLengths.XLarge (128 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.VendorName).ValidateVendorName();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string> ValidateVendorName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Vendor name is required")
            .MaximumLength(AccountingStringLengths.XLarge)
            .WithMessage($"Vendor name must not exceed {AccountingStringLengths.XLarge} characters");
    }

    // ========== Tax Code Validations ==========

    /// <summary>
    /// Applies validation rules for tax code property.
    /// 
    /// Rules:
    /// - Must not be empty
    /// - Must not exceed AccountingStringLengths.Regular (16 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.TaxCode).ValidateTaxCode();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string> ValidateTaxCode<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Tax code is required")
            .MaximumLength(AccountingStringLengths.Regular)
            .WithMessage($"Tax code must not exceed {AccountingStringLengths.Regular} characters");
    }

    // ========== Amount Validations ==========

    /// <summary>
    /// Applies validation rules for monetary amount property.
    /// 
    /// Rules:
    /// - Must be greater than or equal to 0
    /// 
    /// Usage:
    /// RuleFor(x => x.Amount).ValidateAmount();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, decimal> ValidateAmount<T>(
        this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(0)
            .WithMessage("Amount must be greater than or equal to 0");
    }

    /// <summary>
    /// Applies validation rules for positive monetary amount property.
    /// 
    /// Rules:
    /// - Must be greater than 0
    /// 
    /// Usage:
    /// RuleFor(x => x.Amount).ValidatePositiveAmount();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, decimal> ValidatePositiveAmount<T>(
        this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");
    }

    // ========== Reference Number Validations ==========

    /// <summary>
    /// Applies validation rules for reference number property.
    /// 
    /// Rules:
    /// - Optional field
    /// - Must not exceed AccountingStringLengths.Large (64 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.ReferenceNumber).ValidateReferenceNumber();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string?> ValidateReferenceNumber<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
        where T : class
    {
        return ruleBuilder
            .MaximumLength(AccountingStringLengths.Large)
            .WithMessage($"Reference number must not exceed {AccountingStringLengths.Large} characters")
            .When(x => x != null);
    }
}
