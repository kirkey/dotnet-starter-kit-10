using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.InterconnectionAgreements.CreateInterconnectionAgreement;

public class CreateInterconnectionAgreementValidator : AbstractValidator<CreateInterconnectionAgreementCommand>
{
    public CreateInterconnectionAgreementValidator()
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
