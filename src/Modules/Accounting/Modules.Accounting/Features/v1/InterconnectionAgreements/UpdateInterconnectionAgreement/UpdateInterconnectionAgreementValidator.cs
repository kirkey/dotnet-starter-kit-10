using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.InterconnectionAgreements.UpdateInterconnectionAgreement;

public class UpdateInterconnectionAgreementValidator : AbstractValidator<UpdateInterconnectionAgreementCommand>
{
    public UpdateInterconnectionAgreementValidator()
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
