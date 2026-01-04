using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.RecurringJournalEntries.ApproveRecurringJournalEntry;

public class ApproveRecurringJournalEntryValidator : AbstractValidator<ApproveRecurringJournalEntryCommand>
{
    public ApproveRecurringJournalEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
