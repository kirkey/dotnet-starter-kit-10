using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.ApproveJournalEntry;

public class ApproveJournalEntryValidator : AbstractValidator<ApproveJournalEntryCommand>
{
    public ApproveJournalEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
