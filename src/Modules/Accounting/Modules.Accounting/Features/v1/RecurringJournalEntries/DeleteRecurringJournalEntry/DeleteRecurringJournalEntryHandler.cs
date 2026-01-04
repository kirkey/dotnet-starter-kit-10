using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.RecurringJournalEntries.DeleteRecurringJournalEntry;

public record DeleteRecurringJournalEntryCommand(Guid Id) : ICommand;

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
