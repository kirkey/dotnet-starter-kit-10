using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.DepreciationMethods.CreateDepreciationMethod;

public class CreateDepreciationMethodValidator : AbstractValidator<CreateDepreciationMethodCommand>
{
    public CreateDepreciationMethodValidator()
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
