using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.DebitMemos.UpdateDebitMemo;

namespace FSH.Module.Accounting.Features.v1.DebitMemos.UpdateDebitMemo;

public class UpdateDebitMemoValidator : AbstractValidator<UpdateDebitMemoCommand>
{
    public UpdateDebitMemoValidator()
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
