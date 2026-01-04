using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.CreateJournalEntry;

public record CreateJournalEntryCommand(
    string EntryNumber,
    DateTime EntryDate,
    string EntryType,
    string ReferenceNumber,
    Guid FiscalPeriodId,
    string? ReferenceType = null,
    string? Description = null,
    string? Notes = null,
    string? Memo = null) : ICommand<Guid>;

public class CreateJournalEntryHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateJournalEntryCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateJournalEntryCommand command, CancellationToken ct)
    {
        var entity = JournalEntry.Create(
            command.EntryNumber,
            command.EntryDate,
            command.EntryType,
            command.ReferenceNumber,
            command.FiscalPeriodId,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.ReferenceType,
            command.Description,
            command.Notes,
            command.Memo);
        
        context.JournalEntries.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
