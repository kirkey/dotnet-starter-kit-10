using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.AccountReconciliations.CreateAccountReconciliation;

namespace FSH.Module.Accounting.Features.v1.AccountReconciliations.CreateAccountReconciliation;

public class CreateAccountReconciliationValidator : AbstractValidator<CreateAccountReconciliationCommand>
{
    public CreateAccountReconciliationValidator()
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
