using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.TrialBalance.ExportTrialBalance;

public class ExportTrialBalanceValidator : AbstractValidator<ExportTrialBalanceQuery>
{
    public ExportTrialBalanceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Format).NotEmpty();
    }
}
