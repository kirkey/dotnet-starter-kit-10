using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.CreateRetainedEarnings;

public class CreateRetainedEarningsValidator : AbstractValidator<CreateRetainedEarningsCommand>
{
    public CreateRetainedEarningsValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
