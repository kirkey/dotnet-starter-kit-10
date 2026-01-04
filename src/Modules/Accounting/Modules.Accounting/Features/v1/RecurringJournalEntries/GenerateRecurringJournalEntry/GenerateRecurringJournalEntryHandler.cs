using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.RecurringJournalEntries.GenerateRecurringJournalEntry;

public record GenerateRecurringJournalEntryCommand(Guid Id) : ICommand;

public class GenerateRecurringJournalEntryHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<GenerateRecurringJournalEntryCommand>
{
    public async ValueTask<Unit> Handle(GenerateRecurringJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.RecurringJournalEntries.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("RecurringJournalEntry not found");

        var journalEntry = entity.Generate(currentUser.GetUserId(), currentUser.Name ?? "System");

        context.JournalEntries.Add(journalEntry);

        // Save both the generated journal entry and the updated recurring entity (last/next run)
        await context.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
