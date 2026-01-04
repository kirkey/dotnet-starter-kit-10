using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.DepreciationMethods.UpdateDepreciationMethod;

public class UpdateDepreciationMethodValidator : AbstractValidator<UpdateDepreciationMethodCommand>
{
    public UpdateDepreciationMethodValidator()
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
