using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.JournalEntries.ReverseJournalEntry;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.ReverseJournalEntry;

public class ReverseJournalEntryValidator : AbstractValidator<ReverseJournalEntryCommand>
{
    public ReverseJournalEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
