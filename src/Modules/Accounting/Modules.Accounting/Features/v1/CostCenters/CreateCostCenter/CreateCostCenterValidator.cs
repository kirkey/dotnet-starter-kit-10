using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.CostCenters.CreateCostCenter;

public class CreateCostCenterValidator : AbstractValidator<CreateCostCenterCommand>
{
    public CreateCostCenterValidator()
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
