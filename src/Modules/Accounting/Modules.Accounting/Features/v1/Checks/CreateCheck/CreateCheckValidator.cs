using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.Checks.CreateCheck;

public class CreateCheckValidator : AbstractValidator<CreateCheckCommand>
{
    public CreateCheckValidator()
    {
        RuleFor(x => x.CheckNumber).NotEmpty().MaximumLength(AccountingStringLengths.CheckNumber);
        RuleFor(x => x.CheckDate).NotEmpty();
        RuleFor(x => x.CheckType).NotEmpty().MaximumLength(AccountingStringLengths.Medium);
        RuleFor(x => x.BankAccountId).NotEmpty();
        RuleFor(x => x.AccountNumber).NotEmpty().MaximumLength(AccountingStringLengths.Medium);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.PayeeName).NotEmpty().MaximumLength(AccountingStringLengths.Name);
        When(x => !string.IsNullOrEmpty(x.ReferenceNumber), () => { RuleFor(x => x.ReferenceNumber).MaximumLength(AccountingStringLengths.Medium); });
        When(x => !string.IsNullOrEmpty(x.Notes), () => { RuleFor(x => x.Notes).MaximumLength(AccountingStringLengths.XHuge); });
    }
}