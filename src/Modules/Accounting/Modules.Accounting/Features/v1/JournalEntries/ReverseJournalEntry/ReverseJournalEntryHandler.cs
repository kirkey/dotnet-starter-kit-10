using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.JournalEntries.ReverseJournalEntry;

public record ReverseJournalEntryCommand(Guid Id) : ICommand;

public class ReverseJournalEntryHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<ReverseJournalEntryCommand>
{
    public async ValueTask<Unit> Handle(ReverseJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.JournalEntries
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("JournalEntry not found");

        if (entity.Status != "Posted" && entity.Status != "Approved")
            throw new BadRequestException("Only posted or approved entries can be reversed");

        // Create reversal entry
        var reversingEntry = JournalEntry.Create(
            entryNumber: entity.EntryNumber + "-REV",
            entryDate: DateTime.UtcNow,
            entryType: "Reversing",
            referenceNumber: entity.ReferenceNumber,
            fiscalPeriodId: entity.FiscalPeriodId,
            tenantId: entity.TenantId,
            createdBy: currentUser.GetUserId(),
            createdByUserName: currentUser.Name ?? "System",
            referenceType: "Reversal",
            description: $"Reversal of {entity.EntryNumber}");

        context.JournalEntries.Add(reversingEntry);

        int lineNo = 1;
        foreach (var line in entity.Lines!.OrderBy(l => l.LineNumber))
        {
            var transactionType = line.TransactionType.Equals("Debit", StringComparison.OrdinalIgnoreCase) ? "Credit" : "Debit";
            var revLine = JournalEntryLine.Create(
                reversingEntry.Id,
                lineNo++,
                line.AccountId,
                line.AccountCode,
                line.AccountName,
                line.Amount,
                transactionType,
                reversingEntry.TenantId,
                currentUser.GetUserId(),
                currentUser.Name ?? "System",
                line.ReferenceNumber,
                line.Description,
                line.Notes,
                line.CostCenterId,
                line.DepartmentId,
                line.ProjectId);

            context.JournalEntryLines.Add(revLine);
        }

        // Update totals and save
        var totalDebit = await context.JournalEntryLines
            .Where(l => l.JournalEntryId == reversingEntry.Id)
            .SumAsync(l => l.Debit, ct);
        var totalCredit = await context.JournalEntryLines
            .Where(l => l.JournalEntryId == reversingEntry.Id)
            .SumAsync(l => l.Credit, ct);

        reversingEntry.UpdateTotals(totalDebit, totalCredit);

        // Mark original as reversed
        entity.Reverse(reversingEntry.Id, DateTime.UtcNow);

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
