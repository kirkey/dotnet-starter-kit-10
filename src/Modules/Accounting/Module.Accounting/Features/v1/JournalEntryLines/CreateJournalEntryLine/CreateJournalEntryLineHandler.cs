using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.JournalEntryLines.CreateJournalEntryLine;

public record CreateJournalEntryLineCommand(
    Guid JournalEntryId,
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

public class CreateJournalEntryLineHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateJournalEntryLineCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateJournalEntryLineCommand command, CancellationToken ct)
    {
        var entity = JournalEntryLine.Create(
            command.JournalEntryId,
            command.LineNumber,
            command.AccountId,
            command.AccountCode,
            command.AccountName,
            command.Amount,
            command.TransactionType,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.ReferenceNumber,
            command.Description,
            command.Notes,
            command.CostCenterId,
            command.DepartmentId,
            command.ProjectId);
        
        context.JournalEntryLines.Add(entity);

        // Update parent journal entry totals
        var totalDebit = await context.JournalEntryLines.Where(l => l.JournalEntryId == entity.JournalEntryId).SumAsync(l => l.Debit, ct);
        var totalCredit = await context.JournalEntryLines.Where(l => l.JournalEntryId == entity.JournalEntryId).SumAsync(l => l.Credit, ct);

        var parent = await context.JournalEntries.FindAsync(entity.JournalEntryId, ct);
        if (parent != null)
        {
            parent.UpdateTotals(totalDebit, totalCredit);
        }

        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
