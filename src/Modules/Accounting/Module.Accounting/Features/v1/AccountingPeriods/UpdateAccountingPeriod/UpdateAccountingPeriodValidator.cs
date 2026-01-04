using FluentValidation;

using FSH.Module.Accounting.Contracts.v1.AccountingPeriods.UpdateAccountingPeriod;

namespace FSH.Module.Accounting.Features.v1.AccountingPeriods.UpdateAccountingPeriod;

public class UpdateAccountingPeriodValidator : AbstractValidator<UpdateAccountingPeriodCommand>
{
    public UpdateAccountingPeriodValidator()
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
