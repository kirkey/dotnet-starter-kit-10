using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.TrialBalance.ExportTrialBalance;

public class ExportTrialBalanceValidator : AbstractValidator<ExportTrialBalanceCommand>
{
    public ExportTrialBalanceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
