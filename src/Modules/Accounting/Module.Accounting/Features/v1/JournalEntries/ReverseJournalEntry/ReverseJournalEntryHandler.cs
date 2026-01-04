using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;
using FSH.Module.Accounting.Contracts.v1.JournalEntries.ReverseJournalEntry;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.ReverseJournalEntry;

/// <summary>
/// Reverse Journal Entry command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to reverse a Journal Entry that has been Posted or Approved.
/// Reversal creates a mirror entry with opposite debit/credit amounts, maintaining audit trail.
/// 
/// **Parameters:**
/// - Id: The Journal Entry ID to reverse
/// 
/// **Business Constraints:**
/// - Entry must be in Posted or Approved status (Draft entries are deleted instead)
/// - Reversal creates a new entry, doesn't modify the original
/// - Original entry is marked as reversed and linked to reversal entry
/// - GL account balances are reversed via the new reversing entry
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// </summary>
/// <summary>
/// Handler for reversing a Journal Entry.
/// 
/// **Responsibility:**
/// Reverses a posted or approved Journal Entry by creating a mirror entry with opposite amounts.
/// Maintains complete audit trail and links original to reversal entry.
/// 
/// **Execution Flow:**
/// 1. Find JournalEntry by ID including line items
/// 2. Verify entry is Posted or Approved (cannot reverse Draft)
/// 3. Create new reversing entry with:
///    - Entry number: {Original}-REV
///    - Entry type: "Reversing"
///    - Reference type: "Reversal"
///    - Description: "Reversal of {OriginalNumber}"
/// 4. For each line item in original:
///    - Create reversal line with swapped TransactionType (Debit↔Credit)
///    - Same amount, same accounts, opposite direction
/// 5. Calculate totals for reversal entry
/// 6. Mark original entry as reversed with reversal entry ID
/// 7. Persist both entries to database
/// 8. Return Unit.Value
/// 
/// **Reversal Mechanics:**
/// - All debit amounts become credit amounts in reversal entry
/// - All credit amounts become debit amounts in reversal entry
/// - GL account balances are reversed (opposite direction)
/// - Cost center, department, project allocations are maintained
/// - Original entry remains intact and unchanged (only marked as reversed)
/// 
/// **Audit Trail:**
/// - Original entry: Status remains "Posted"/"Approved", IsReversed=true
/// - Reversal entry: Status="Draft", created by current user
/// - Link: ReversedEntryId points to new reversal entry
/// - Timestamp: ReversedDate = current UTC time
/// - Both entries preserved for audit compliance
/// 
/// **Permissions:**
/// Requires: Accounting.JournalEntry.Reverse
/// 
/// **Business Rules:**
/// - Can only reverse Posted or Approved entries
/// - Cannot reverse Draft entries (delete instead)
/// - Cannot reverse entries already marked as reversed
/// - Reversal creates new draft entry that must be posted separately
/// - GL account balances updated in reverse direction
/// - May require approval hierarchy for reversal
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when entry not found
/// - BadRequestException: Thrown if entry is not Posted or Approved
/// </summary>
public class ReverseJournalEntryHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<ReverseJournalEntryCommand>
{
    /// <summary>
    /// Handles the ReverseJournalEntryCommand to reverse a Journal Entry.
    /// </summary>
    /// <param name="command">The command containing the Journal Entry ID to reverse</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>Unit.Value on successful reversal (creates new reversal entry)</returns>
    /// <exception cref="NotFoundException">Thrown when JournalEntry is not found</exception>
    /// <exception cref="BadRequestException">Thrown if entry is not in Posted or Approved status</exception>
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
