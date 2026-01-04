using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.CreditMemos.CreateCreditMemo;

public class CreateCreditMemoValidator : AbstractValidator<CreateCreditMemoCommand>
{
    public CreateCreditMemoValidator()
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
