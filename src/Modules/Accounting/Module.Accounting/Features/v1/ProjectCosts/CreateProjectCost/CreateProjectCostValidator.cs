using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.ProjectCosts.CreateProjectCost;

namespace FSH.Module.Accounting.Features.v1.ProjectCosts.CreateProjectCost;

public class CreateProjectCostValidator : AbstractValidator<CreateProjectCostCommand>
{
    public CreateProjectCostValidator()
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
