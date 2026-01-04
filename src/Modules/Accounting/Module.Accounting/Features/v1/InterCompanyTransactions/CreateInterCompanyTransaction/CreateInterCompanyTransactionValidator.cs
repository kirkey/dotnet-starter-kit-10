using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.InterCompanyTransactions.CreateInterCompanyTransaction;

public class CreateInterCompanyTransactionValidator : AbstractValidator<CreateInterCompanyTransactionCommand>
{
    public CreateInterCompanyTransactionValidator()
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
