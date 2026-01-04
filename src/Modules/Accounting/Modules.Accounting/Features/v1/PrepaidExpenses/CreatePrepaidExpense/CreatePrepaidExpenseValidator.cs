using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.PrepaidExpenses.CreatePrepaidExpense;

public class CreatePrepaidExpenseValidator : AbstractValidator<CreatePrepaidExpenseCommand>
{
    public CreatePrepaidExpenseValidator()
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
