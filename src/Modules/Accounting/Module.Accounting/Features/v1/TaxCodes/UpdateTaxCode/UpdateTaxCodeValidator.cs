using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.TaxCodes.UpdateTaxCode;

public class UpdateTaxCodeValidator : AbstractValidator<UpdateTaxCodeCommand>
{
    public UpdateTaxCodeValidator()
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
