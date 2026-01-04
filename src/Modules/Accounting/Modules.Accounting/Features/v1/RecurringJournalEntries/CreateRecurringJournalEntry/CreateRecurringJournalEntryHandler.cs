using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.RecurringJournalEntries.CreateRecurringJournalEntry;

public record CreateRecurringJournalEntryCommand(
    string Name,
    string Frequency,
    DateTime? NextRunDate = null,
    Guid? FiscalPeriodId = null,
    bool IsAutoPost = false,
    string? Description = null) : ICommand<Guid>;

public class CreateRecurringJournalEntryHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateRecurringJournalEntryCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateRecurringJournalEntryCommand command, CancellationToken ct)
    {
        var entity = RecurringJournalEntry.Create(
            command.Name,
            command.Frequency,
            command.NextRunDate,
            command.FiscalPeriodId,
            command.IsAutoPost,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.RecurringJournalEntries.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
