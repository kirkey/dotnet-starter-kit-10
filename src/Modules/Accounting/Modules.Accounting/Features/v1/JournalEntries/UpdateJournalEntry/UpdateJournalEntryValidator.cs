using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.JournalEntries.UpdateJournalEntry;

public class UpdateJournalEntryValidator : AbstractValidator<UpdateJournalEntryCommand>
{
    public UpdateJournalEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        
        RuleFor(x => x.EntryNumber)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.JournalEntryNumber);
            
        RuleFor(x => x.EntryDate)
            .NotEmpty();
            
        RuleFor(x => x.EntryType)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Medium)
            .Must(BeValidEntryType).WithMessage("EntryType must be Standard, Adjusting, Closing, or Reversing");
            
        RuleFor(x => x.ReferenceNumber)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Medium);
            
        RuleFor(x => x.FiscalPeriodId)
            .NotEmpty();
            
        When(x => !string.IsNullOrEmpty(x.ReferenceType), () =>
        {
            RuleFor(x => x.ReferenceType)
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
            
        When(x => !string.IsNullOrEmpty(x.Memo), () =>
        {
            RuleFor(x => x.Memo)
                .MaximumLength(AccountingStringLengths.Large);
        });
    }
    
    private bool BeValidEntryType(string entryType)
    {
        var validTypes = new[] { "Standard", "Adjusting", "Closing", "Reversing" };
        return validTypes.Contains(entryType, StringComparer.OrdinalIgnoreCase);
    }
}
