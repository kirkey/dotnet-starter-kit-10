using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.CostCenters.UpdateCostCenter;

namespace FSH.Module.Accounting.Features.v1.CostCenters.UpdateCostCenter;

public class UpdateCostCenterValidator : AbstractValidator<UpdateCostCenterCommand>
{
    public UpdateCostCenterValidator()
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
