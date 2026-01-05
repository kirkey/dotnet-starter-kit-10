using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Budgets.ApproveBudget;

namespace FSH.Module.Accounting.Features.v1.Budgets.ApproveBudget;

public class ApproveBudgetValidator : AbstractValidator<ApproveBudgetCommand>
{
    public ApproveBudgetValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
