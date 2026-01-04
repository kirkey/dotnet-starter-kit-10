using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.Payees.UpdatePayee;

public class UpdatePayeeValidator : AbstractValidator<UpdatePayeeCommand>
{
    public UpdatePayeeValidator()
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
