using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.PostJournalEntry;

public class PostJournalEntryValidator : AbstractValidator<PostJournalEntryCommand>
{
    public PostJournalEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
