using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.SecurityDeposits.CreateSecurityDeposit;

public class CreateSecurityDepositValidator : AbstractValidator<CreateSecurityDepositCommand>
{
    public CreateSecurityDepositValidator()
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
