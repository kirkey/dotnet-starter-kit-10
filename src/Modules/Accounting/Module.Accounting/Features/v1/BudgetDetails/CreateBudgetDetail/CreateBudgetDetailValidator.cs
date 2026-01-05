using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.BudgetDetails.CreateBudgetDetail;

namespace FSH.Module.Accounting.Features.v1.BudgetDetails.CreateBudgetDetail;

public class CreateBudgetDetailValidator : AbstractValidator<CreateBudgetDetailCommand>
{
    public CreateBudgetDetailValidator()
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
