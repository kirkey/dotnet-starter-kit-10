using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Budgets.UpdateBudget;

namespace FSH.Module.Accounting.Features.v1.Budgets.UpdateBudget;

public class UpdateBudgetValidator : AbstractValidator<UpdateBudgetCommand>
{
    public UpdateBudgetValidator()
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
