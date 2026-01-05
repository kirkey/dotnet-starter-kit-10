using FSH.Module.Microfinance.Contracts.v1.Members;

namespace FSH.Module.Microfinance.Features.v1.Members.CreateMember;

/// <summary>
/// Validator for CreateMemberCommand.
/// 
/// **Purpose:**
/// Validates all member creation requests to ensure data quality and business rules compliance.
/// 
/// **Validation Rules:**
/// - Member number: Required, max 64 chars, alphanumeric with hyphens
/// - First name: Required, 2-128 chars, letters/spaces/hyphens/apostrophes only
/// - Last name: Required, 2-128 chars, letters/spaces/hyphens/apostrophes only
/// - Middle name: Optional, max 128 chars
/// - Email: Optional, valid email format, max 256 chars
/// - Phone number: Optional, max 32 chars, digits/spaces/parentheses/plus/hyphens
/// - Gender: Optional, max 32 chars
/// - Address: Optional, max 512 chars
/// - National ID: Optional, max 64 chars, alphanumeric with hyphens
/// - Occupation: Optional, max 256 chars
/// </summary>
public class CreateMemberValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.MemberNumber)
            .ValidateMemberNumber();

        RuleFor(x => x.FirstName)
            .ValidateMemberName();

        RuleFor(x => x.LastName)
            .ValidateMemberName();

        RuleFor(x => x.MiddleName)
            .ValidateOptionalMemberName();

        RuleFor(x => x.Email)
            .ValidateEmail();

        RuleFor(x => x.PhoneNumber)
            .ValidatePhoneNumber();

        RuleFor(x => x.Gender)
            .MaximumLength(MicrofinanceStringLengths.MemberGender)
            .When(x => !string.IsNullOrWhiteSpace(x.Gender));

        RuleFor(x => x.Address)
            .MaximumLength(MicrofinanceStringLengths.MemberAddress)
            .When(x => !string.IsNullOrWhiteSpace(x.Address));

        RuleFor(x => x.NationalId)
            .ValidateNationalId();

        RuleFor(x => x.Occupation)
            .MaximumLength(MicrofinanceStringLengths.MemberOccupation)
            .When(x => !string.IsNullOrWhiteSpace(x.Occupation));

        RuleFor(x => x.MonthlyIncome)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MonthlyIncome.HasValue);

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTimeOffset.UtcNow)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be in the past");
    }
}
