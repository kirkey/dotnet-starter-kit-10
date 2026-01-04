using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.DeleteJournalEntry;

public record DeleteJournalEntryCommand(Guid Id) : ICommand;

public class DeleteJournalEntryHandler(AccountingDbContext context) : ICommandHandler<DeleteJournalEntryCommand>
{
    public async ValueTask<Unit> Handle(DeleteJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.JournalEntries.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("JournalEntry not found");
        
        if (entity.Status == "Posted" || entity.Status == "Approved")
            throw new BadRequestException("Cannot delete a posted or approved journal entry");
        
        context.JournalEntries.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
