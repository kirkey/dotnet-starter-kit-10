using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.UpdateRecurringJournalEntry;

/// <summary>
/// Update Recurring Journal Entry command.
/// </summary>
/// <param name="Id">Recurring journal entry ID to update</param>
/// <param name="Name">Updated name/description</param>
/// <param name="Frequency">Updated frequency</param>
/// <param name="NextRunDate">Updated next run date or null</param>
/// <param name="FiscalPeriodId">Updated fiscal period ID or null</param>
/// <param name="IsAutoPost">Updated auto-post setting</param>
/// <param name="Description">Updated description or null</param>
public record UpdateRecurringJournalEntryCommand(
    Guid Id,
    string Name,
    string Frequency,
    DateTime? NextRunDate = null,
    Guid? FiscalPeriodId = null,
    bool IsAutoPost = false,
    string? Description = null) : ICommand<Guid>;