using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.ApproveRecurringJournalEntry;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.ApproveRecurringJournalEntry;

public class ApproveRecurringJournalEntryValidator : AbstractValidator<ApproveRecurringJournalEntryCommand>
{
    public ApproveRecurringJournalEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
