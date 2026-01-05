using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.ProjectCosts.UpdateProjectCost;

namespace FSH.Module.Accounting.Features.v1.ProjectCosts.UpdateProjectCost;

public class UpdateProjectCostValidator : AbstractValidator<UpdateProjectCostCommand>
{
    public UpdateProjectCostValidator()
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
