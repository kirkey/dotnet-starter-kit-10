using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.AccountingPeriods.CreateAccountingPeriod;

public class CreateAccountingPeriodValidator : AbstractValidator<CreateAccountingPeriodCommand>
{
    public CreateAccountingPeriodValidator()
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
