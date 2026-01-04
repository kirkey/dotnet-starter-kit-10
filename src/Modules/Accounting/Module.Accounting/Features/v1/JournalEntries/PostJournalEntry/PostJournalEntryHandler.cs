using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Context;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.PostJournalEntry;

public record PostJournalEntryCommand(Guid Id) : ICommand;

public class PostJournalEntryHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<PostJournalEntryCommand>
{
    public async ValueTask<Unit> Handle(PostJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.JournalEntries
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("JournalEntry not found");

        // Recalculate totals from lines
        var totalDebit = entity.Lines?.Sum(l => l.Debit) ?? 0m;
        var totalCredit = entity.Lines?.Sum(l => l.Credit) ?? 0m;

        entity.UpdateTotals(totalDebit, totalCredit);

        if (!entity.IsBalanced())
            throw new BadRequestException("Journal entry is not balanced. Debits must equal credits.");

        entity.Post(currentUser.GetUserId(), DateTime.UtcNow);

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
