using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Budgets.CreateBudget;

namespace FSH.Module.Accounting.Features.v1.Budgets.CreateBudget;

public class CreateBudgetValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetValidator()
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
