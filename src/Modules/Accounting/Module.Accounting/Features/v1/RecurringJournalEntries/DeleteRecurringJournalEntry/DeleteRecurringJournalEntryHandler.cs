using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.DeleteRecurringJournalEntry;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.DeleteRecurringJournalEntry;

public class DeleteRecurringJournalEntryHandler(AccountingDbContext context) : ICommandHandler<DeleteRecurringJournalEntryCommand>
{
    public async ValueTask<Unit> Handle(DeleteRecurringJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.RecurringJournalEntries.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("RecurringJournalEntry not found");
        
        context.RecurringJournalEntries.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
