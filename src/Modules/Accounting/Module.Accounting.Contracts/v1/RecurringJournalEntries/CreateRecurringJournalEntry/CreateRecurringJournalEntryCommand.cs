using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.CreateRecurringJournalEntry;

/// <summary>
/// Create Recurring Journal Entry command.
/// </summary>
/// <param name="Name">Name/description of the recurring entry</param>
/// <param name="Frequency">Recurrence frequency (e.g., "Monthly", "Quarterly", "Annually")</param>
/// <param name="NextRunDate">Optional next run date</param>
/// <param name="FiscalPeriodId">Optional fiscal period ID</param>
/// <param name="IsAutoPost">Whether to auto-post generated entries</param>
/// <param name="Description">Optional description</param>
public record CreateRecurringJournalEntryCommand(
    string Name,
    string Frequency,
    DateTime? NextRunDate = null,
    Guid? FiscalPeriodId = null,
    bool IsAutoPost = false,
    string? Description = null) : ICommand<Guid>;
