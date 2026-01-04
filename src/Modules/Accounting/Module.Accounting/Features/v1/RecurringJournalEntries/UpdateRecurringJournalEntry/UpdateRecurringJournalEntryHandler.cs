using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.UpdateRecurringJournalEntry;

public record UpdateRecurringJournalEntryCommand(Guid Id, string Name, string Frequency, DateTime? NextRunDate = null, Guid? FiscalPeriodId = null, bool IsAutoPost = false, string? Description = null) : ICommand<Guid>;

public class UpdateRecurringJournalEntryHandler(AccountingDbContext context) : ICommandHandler<UpdateRecurringJournalEntryCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateRecurringJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.RecurringJournalEntries.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("RecurringJournalEntry not found");
        
        entity.Update(command.Name, command.Frequency, command.NextRunDate, command.FiscalPeriodId, command.IsAutoPost, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
