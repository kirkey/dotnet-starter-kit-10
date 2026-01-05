using FluentValidation;

namespace FSH.Module.Microfinance.Features;

/// <summary>
/// Extension methods for FluentValidation rules used across Microfinance module.
/// 
/// **Purpose:**
/// Provides reusable, consistent validation rules for common fields and patterns.
/// Reduces code duplication and ensures consistent validation across all validators.
/// 
/// **Benefits:**
/// - DRY (Don't Repeat Yourself) principle
/// - Consistent validation rules and error messages
/// - Centralized business rule definitions
/// - Easier maintenance and updates
/// - Testable validation logic
/// 
/// **Usage Examples:**
/// RuleFor(x => x.FirstName).ValidateMemberName();
/// RuleFor(x => x.Email).ValidateEmail();
/// RuleFor(x => x.PhoneNumber).ValidatePhoneNumber();
/// </summary>
public static class MicrofinanceValidationExtensions
{
    /// <summary>
    /// Validates member name fields (FirstName, LastName, MiddleName).
    /// 
    /// **Rules:**
    /// - Not empty
    /// - Between 2 and 128 characters
    /// - Only letters, spaces, hyphens, and apostrophes
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateMemberName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .MinimumLength(MicrofinanceStringLengths.MemberFirstNameMinLength)
            .MaximumLength(MicrofinanceStringLengths.MemberFirstName)
            .Matches(@"^[a-zA-Z\s\-']+$")
            .WithMessage("Name can only contain letters, spaces, hyphens, and apostrophes.");
    }

    /// <summary>
    /// Validates optional member name fields.
    /// 
    /// **Rules:**
    /// - Can be null or empty
    /// - If provided, max 128 characters
    /// - Only letters, spaces, hyphens, and apostrophes
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateOptionalMemberName<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(MicrofinanceStringLengths.MemberMiddleName)
            .Matches(@"^[a-zA-Z\s\-']*$")
            .When(x => !string.IsNullOrWhiteSpace(ruleBuilder.ToString()))
            .WithMessage("Name can only contain letters, spaces, hyphens, and apostrophes.");
    }

    /// <summary>
    /// Validates email address.
    /// 
    /// **Rules:**
    /// - Valid email format
    /// - Max 256 characters
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateEmail<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .EmailAddress()
            .MaximumLength(MicrofinanceStringLengths.MemberEmail)
            .When(x => !string.IsNullOrWhiteSpace(ruleBuilder.ToString()));
    }

    /// <summary>
    /// Validates phone number.
    /// 
    /// **Rules:**
    /// - Max 32 characters
    /// - Only digits, spaces, parentheses, plus, and hyphens
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidatePhoneNumber<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(MicrofinanceStringLengths.MemberPhoneNumber)
            .Matches(@"^[\d\s\(\)\+\-]*$")
            .When(x => !string.IsNullOrWhiteSpace(ruleBuilder.ToString()))
            .WithMessage("Phone number can only contain digits, spaces, parentheses, plus, and hyphens.");
    }

    /// <summary>
    /// Validates member number.
    /// 
    /// **Rules:**
    /// - Not empty
    /// - Max 64 characters
    /// - Alphanumeric with hyphens
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateMemberNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .MaximumLength(MicrofinanceStringLengths.MemberNumber)
            .Matches(@"^[a-zA-Z0-9\-]+$")
            .WithMessage("Member number can only contain letters, numbers, and hyphens.");
    }

    /// <summary>
    /// Validates national ID.
    /// 
    /// **Rules:**
    /// - Max 64 characters
    /// - Alphanumeric with hyphens
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateNationalId<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(MicrofinanceStringLengths.MemberNationalId)
            .Matches(@"^[a-zA-Z0-9\-]*$")
            .When(x => !string.IsNullOrWhiteSpace(ruleBuilder.ToString()))
            .WithMessage("National ID can only contain letters, numbers, and hyphens.");
    }

    /// <summary>
    /// Validates loan/account amount.
    /// 
    /// **Rules:**
    /// - Must be greater than zero
    /// - Max 2 decimal places
    /// </summary>
    public static IRuleBuilderOptions<T, decimal> ValidateAmount<T>(
        this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0)
            .PrecisionScale(18, 2, ignoreTrailingZeros: true)
            .WithMessage("Amount must be greater than zero with max 2 decimal places.");
    }

    /// <summary>
    /// Validates interest rate percentage.
    /// 
    /// **Rules:**
    /// - Must be between 0 and 100
    /// - Max 4 decimal places
    /// </summary>
    public static IRuleBuilderOptions<T, decimal> ValidateInterestRate<T>(
        this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
            .InclusiveBetween(0, 100)
            .PrecisionScale(5, 4, ignoreTrailingZeros: true)
            .WithMessage("Interest rate must be between 0 and 100 with max 4 decimal places.");
    }

    /// <summary>
    /// Validates generic name field.
    /// 
    /// **Rules:**
    /// - Not empty
    /// - Max 256 characters
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .MaximumLength(MicrofinanceStringLengths.Name);
    }

    /// <summary>
    /// Validates generic description field.
    /// 
    /// **Rules:**
    /// - Max 512 characters
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(MicrofinanceStringLengths.Description)
            .When(x => !string.IsNullOrWhiteSpace(ruleBuilder.ToString()));
    }

    /// <summary>
    /// Validates generic notes field.
    /// 
    /// **Rules:**
    /// - Max 2048 characters
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateNotes<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(MicrofinanceStringLengths.Notes)
            .When(x => !string.IsNullOrWhiteSpace(ruleBuilder.ToString()));
    }
}
