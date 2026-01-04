using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.JournalEntryLines.CreateJournalEntryLine;

public class CreateJournalEntryLineValidator : AbstractValidator<CreateJournalEntryLineCommand>
{
    public CreateJournalEntryLineValidator()
    {
        RuleFor(x => x.JournalEntryId)
            .NotEmpty();
            
        RuleFor(x => x.LineNumber)
            .GreaterThan(0);
            
        RuleFor(x => x.AccountId)
            .NotEmpty();
            
        RuleFor(x => x.AccountCode)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.AccountCode);
            
        RuleFor(x => x.AccountName)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.AccountName);
            
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0);
            
        RuleFor(x => x.TransactionType)
            .NotEmpty()
            .Must(BeValidTransactionType).WithMessage("TransactionType must be Debit or Credit");
            
        When(x => !string.IsNullOrEmpty(x.ReferenceNumber), () =>
        {
            RuleFor(x => x.ReferenceNumber)
                .MaximumLength(AccountingStringLengths.Medium);
        });
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
            
        When(x => !string.IsNullOrEmpty(x.Notes), () =>
        {
            RuleFor(x => x.Notes)
                .MaximumLength(AccountingStringLengths.XHuge);
        });
    }
    
    private bool BeValidTransactionType(string transactionType)
    {
        return transactionType.Equals("Debit", StringComparison.OrdinalIgnoreCase) ||
               transactionType.Equals("Credit", StringComparison.OrdinalIgnoreCase);
    }
}
