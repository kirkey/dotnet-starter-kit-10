using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.Banks.CreateBank;

public class CreateBankValidator : AbstractValidator<CreateBankCommand>
{
    public CreateBankValidator()
    {
        RuleFor(x => x.BankName).NotEmpty().MaximumLength(AccountingStringLengths.BankName);
        When(x => !string.IsNullOrEmpty(x.BankCode), () => { RuleFor(x => x.BankCode).MaximumLength(AccountingStringLengths.Medium); });
        When(x => !string.IsNullOrEmpty(x.Address), () => { RuleFor(x => x.Address).MaximumLength(AccountingStringLengths.XHuge); });
        When(x => !string.IsNullOrEmpty(x.ContactName), () => { RuleFor(x => x.ContactName).MaximumLength(AccountingStringLengths.Name); });
        When(x => !string.IsNullOrEmpty(x.ContactPhone), () => { RuleFor(x => x.ContactPhone).MaximumLength(AccountingStringLengths.Medium); });
        When(x => !string.IsNullOrEmpty(x.RoutingNumber), () => { RuleFor(x => x.RoutingNumber).MaximumLength(AccountingStringLengths.Medium); });
        When(x => !string.IsNullOrEmpty(x.SwiftCode), () => { RuleFor(x => x.SwiftCode).MaximumLength(AccountingStringLengths.Medium); });
        When(x => !string.IsNullOrEmpty(x.CurrencyCode), () => { RuleFor(x => x.CurrencyCode).MaximumLength(AccountingStringLengths.Small); });
        RuleFor(x => x.OpeningBalance).GreaterThanOrEqualTo(0);
    }
}
