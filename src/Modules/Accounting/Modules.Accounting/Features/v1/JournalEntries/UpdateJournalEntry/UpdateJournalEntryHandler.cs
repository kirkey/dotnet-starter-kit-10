using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.JournalEntries.UpdateJournalEntry;

public record UpdateJournalEntryCommand(
    Guid Id,
    string EntryNumber,
    DateTime EntryDate,
    string EntryType,
    string ReferenceNumber,
    Guid FiscalPeriodId,
    string? ReferenceType = null,
    string? Description = null,
    string? Notes = null,
    string? Memo = null) : ICommand<Guid>;

public class UpdateJournalEntryHandler(AccountingDbContext context) : ICommandHandler<UpdateJournalEntryCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.JournalEntries.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("JournalEntry not found");
        
        entity.Update(
            command.EntryNumber,
            command.EntryDate,
            command.EntryType,
            command.ReferenceNumber,
            command.FiscalPeriodId,
            command.ReferenceType,
            command.Description,
            command.Notes,
            command.Memo);
        
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
