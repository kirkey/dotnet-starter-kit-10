using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.CreateRecurringJournalEntry;

public class CreateRecurringJournalEntryValidator : AbstractValidator<CreateRecurringJournalEntryCommand>
{
    public CreateRecurringJournalEntryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        RuleFor(x => x.Frequency)
            .NotEmpty()
            .Must(f => new[] { "Daily", "Weekly", "Monthly" }.Contains(f, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Frequency must be Daily, Weekly, or Monthly");

        When(x => x.NextRunDate.HasValue, () =>
        {
            RuleFor(x => x.NextRunDate!.Value)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("NextRunDate cannot be in the past");
        });

        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
