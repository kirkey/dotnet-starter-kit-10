using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.JournalEntries.ApproveJournalEntry;

public class ApproveJournalEntryValidator : AbstractValidator<ApproveJournalEntryCommand>
{
    public ApproveJournalEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
