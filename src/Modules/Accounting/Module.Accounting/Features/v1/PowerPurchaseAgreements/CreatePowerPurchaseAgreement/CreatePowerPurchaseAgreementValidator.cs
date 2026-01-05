using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements.CreatePowerPurchaseAgreement;

namespace FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.CreatePowerPurchaseAgreement;

public class CreatePowerPurchaseAgreementValidator : AbstractValidator<CreatePowerPurchaseAgreementCommand>
{
    public CreatePowerPurchaseAgreementValidator()
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
