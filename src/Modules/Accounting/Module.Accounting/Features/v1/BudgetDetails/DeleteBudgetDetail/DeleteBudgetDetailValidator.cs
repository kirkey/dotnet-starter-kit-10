using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.BudgetDetails.DeleteBudgetDetail;

public class DeleteBudgetDetailValidator : AbstractValidator<DeleteBudgetDetailCommand>
{
    public DeleteBudgetDetailValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
