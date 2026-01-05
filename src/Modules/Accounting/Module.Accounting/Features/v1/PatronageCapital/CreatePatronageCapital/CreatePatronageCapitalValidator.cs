using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.PatronageCapital.CreatePatronageCapital;

namespace FSH.Module.Accounting.Features.v1.PatronageCapital.CreatePatronageCapital;

public class CreatePatronageCapitalValidator : AbstractValidator<CreatePatronageCapitalCommand>
{
    public CreatePatronageCapitalValidator()
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
