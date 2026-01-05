using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.BudgetDetails.DeleteBudgetDetail;

namespace FSH.Module.Accounting.Features.v1.BudgetDetails.DeleteBudgetDetail;

public class DeleteBudgetDetailValidator : AbstractValidator<DeleteBudgetDetailCommand>
{
    public DeleteBudgetDetailValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
