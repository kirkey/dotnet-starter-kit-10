using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.RecurringJournalEntries.GenerateRecurringJournalEntry;

public class GenerateRecurringJournalEntryValidator : AbstractValidator<GenerateRecurringJournalEntryCommand>
{
    public GenerateRecurringJournalEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
