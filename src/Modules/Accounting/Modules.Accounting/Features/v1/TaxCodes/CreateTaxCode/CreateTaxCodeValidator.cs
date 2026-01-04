using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.TaxCodes.CreateTaxCode;

public class CreateTaxCodeValidator : AbstractValidator<CreateTaxCodeCommand>
{
    public CreateTaxCodeValidator()
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
