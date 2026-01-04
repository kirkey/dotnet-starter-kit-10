using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.JournalEntryLines.UpdateJournalEntryLine;

public record UpdateJournalEntryLineCommand(
    Guid Id,
    int LineNumber,
    Guid AccountId,
    string AccountCode,
    string AccountName,
    decimal Amount,
    string TransactionType,
    string? ReferenceNumber = null,
    string? Description = null,
    string? Notes = null,
    Guid? CostCenterId = null,
    Guid? DepartmentId = null,
    Guid? ProjectId = null) : ICommand<Guid>;

public class UpdateJournalEntryLineHandler(AccountingDbContext context) : ICommandHandler<UpdateJournalEntryLineCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateJournalEntryLineCommand command, CancellationToken ct)
    {
        var entity = await context.JournalEntryLines.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("JournalEntryLine not found");
        
        entity.Update(
            command.LineNumber,
            command.AccountId,
            command.AccountCode,
            command.AccountName,
            command.Amount,
            command.TransactionType,
            command.ReferenceNumber,
            command.Description,
            command.Notes,
            command.CostCenterId,
            command.DepartmentId,
            command.ProjectId);
        
        await context.SaveChangesAsync(ct);

        // Recalculate parent totals after update
        var totalDebit = await context.JournalEntryLines.Where(l => l.JournalEntryId == entity.JournalEntryId).SumAsync(l => l.Debit, ct);
        var totalCredit = await context.JournalEntryLines.Where(l => l.JournalEntryId == entity.JournalEntryId).SumAsync(l => l.Credit, ct);

        var parent = await context.JournalEntries.FindAsync(entity.JournalEntryId, ct);
        if (parent != null)
        {
            parent.UpdateTotals(totalDebit, totalCredit);
            await context.SaveChangesAsync(ct);
        }

        return entity.Id;
    }
}
