using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.GenerateRecurringJournalEntry;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.GenerateRecurringJournalEntry;

public class GenerateRecurringJournalEntryValidator : AbstractValidator<GenerateRecurringJournalEntryCommand>
{
    public GenerateRecurringJournalEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
