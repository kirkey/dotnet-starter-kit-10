using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.TrialBalance.GenerateTrialBalance;

public class GenerateTrialBalanceValidator : AbstractValidator<GenerateTrialBalanceCommand>
{
    public GenerateTrialBalanceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
