using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.JournalEntries.ReverseJournalEntry;

public class ReverseJournalEntryValidator : AbstractValidator<ReverseJournalEntryCommand>
{
    public ReverseJournalEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
