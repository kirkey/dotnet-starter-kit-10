using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Budgets.CreateBudget;
using FSH.Module.Accounting.Features;

namespace FSH.Module.Accounting.Features.v1.Budgets.CreateBudget;

public class CreateBudgetValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetValidator()
    {
        RuleFor(x => x.Name)
            .ValidateName();
            
        RuleFor(x => x.Description)
            .ValidateDescription();
    }
}
