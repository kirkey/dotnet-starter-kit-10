using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.Budgets.ApproveBudget;

public class ApproveBudgetValidator : AbstractValidator<ApproveBudgetCommand>
{
    public ApproveBudgetValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
