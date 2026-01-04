using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.ApproveJournalEntry;

public record ApproveJournalEntryCommand(Guid Id) : ICommand;

public class ApproveJournalEntryHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<ApproveJournalEntryCommand>
{
    public async ValueTask<Unit> Handle(ApproveJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.JournalEntries
            .FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("JournalEntry not found");

        if (entity.Status != "Posted")
            throw new BadRequestException("Journal entry must be posted before approval");

        entity.Approve(currentUser.GetUserId(), DateTime.UtcNow);

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
