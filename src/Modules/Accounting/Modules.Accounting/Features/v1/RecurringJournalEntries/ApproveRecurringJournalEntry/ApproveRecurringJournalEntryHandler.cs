using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.RecurringJournalEntries.ApproveRecurringJournalEntry;

public record ApproveRecurringJournalEntryCommand(Guid Id) : ICommand;

public class ApproveRecurringJournalEntryHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<ApproveRecurringJournalEntryCommand>
{
    public async ValueTask<Unit> Handle(ApproveRecurringJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.RecurringJournalEntries.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("RecurringJournalEntry not found");

        entity.Approve(currentUser.GetUserId());

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
