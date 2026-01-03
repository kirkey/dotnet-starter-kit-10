using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.Members.CreateMember;

public class CreateMemberValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.MemberNumber)
            .NotEmpty()
            .MaximumLength(Member.MemberNumberMaxLength);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(Member.FirstNameMinLength)
            .MaximumLength(Member.FirstNameMaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(Member.LastNameMinLength)
            .MaximumLength(Member.LastNameMaxLength);

        RuleFor(x => x.MiddleName)
            .MaximumLength(Member.MiddleNameMaxLength)
            .When(x => !string.IsNullOrEmpty(x.MiddleName));

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(Member.EmailMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(Member.PhoneNumberMaxLength)
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.Gender)
            .MaximumLength(Member.GenderMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Gender));

        RuleFor(x => x.Address)
            .MaximumLength(Member.AddressMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.NationalId)
            .MaximumLength(Member.NationalIdMaxLength)
            .When(x => !string.IsNullOrEmpty(x.NationalId));

        RuleFor(x => x.Occupation)
            .MaximumLength(Member.OccupationMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Occupation));

        RuleFor(x => x.MonthlyIncome)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MonthlyIncome.HasValue);

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTimeOffset.UtcNow)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be in the past");
    }
}
