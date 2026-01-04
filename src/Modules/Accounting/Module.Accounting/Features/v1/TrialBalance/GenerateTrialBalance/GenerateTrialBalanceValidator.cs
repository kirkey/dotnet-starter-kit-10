using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.TrialBalance.GenerateTrialBalance;

public class GenerateTrialBalanceValidator : AbstractValidator<GenerateTrialBalanceCommand>
{
    public GenerateTrialBalanceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
