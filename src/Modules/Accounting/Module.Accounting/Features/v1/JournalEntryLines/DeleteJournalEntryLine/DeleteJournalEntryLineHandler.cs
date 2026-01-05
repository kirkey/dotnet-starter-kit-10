using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.JournalEntryLines.DeleteJournalEntryLine;

namespace FSH.Module.Accounting.Features.v1.JournalEntryLines.DeleteJournalEntryLine;

public class DeleteJournalEntryLineHandler(AccountingDbContext context) : ICommandHandler<DeleteJournalEntryLineCommand>
{
    public async ValueTask<Unit> Handle(DeleteJournalEntryLineCommand command, CancellationToken ct)
    {
        var entity = await context.JournalEntryLines.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("JournalEntryLine not found");
        
        var journalEntryId = entity.JournalEntryId;
        context.JournalEntryLines.Remove(entity);
        await context.SaveChangesAsync(ct);

        // Recalculate parent totals after delete
        var totalDebit = await context.JournalEntryLines.Where(l => l.JournalEntryId == journalEntryId).SumAsync(l => l.Debit, ct);
        var totalCredit = await context.JournalEntryLines.Where(l => l.JournalEntryId == journalEntryId).SumAsync(l => l.Credit, ct);

        var parent = await context.JournalEntries.FindAsync(journalEntryId, ct);
        if (parent != null)
        {
            parent.UpdateTotals(totalDebit, totalCredit);
            await context.SaveChangesAsync(ct);
        }

        return Unit.Value;
    }
}
