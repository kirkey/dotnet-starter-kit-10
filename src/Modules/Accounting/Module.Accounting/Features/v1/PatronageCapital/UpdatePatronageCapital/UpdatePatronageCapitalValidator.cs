using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.PatronageCapital.UpdatePatronageCapital;

namespace FSH.Module.Accounting.Features.v1.PatronageCapital.UpdatePatronageCapital;

public class UpdatePatronageCapitalValidator : AbstractValidator<UpdatePatronageCapitalCommand>
{
    public UpdatePatronageCapitalValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
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
