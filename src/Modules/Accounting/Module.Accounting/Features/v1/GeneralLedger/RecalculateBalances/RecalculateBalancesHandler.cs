using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Contracts.v1.GeneralLedger.RecalculateBalances;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.GeneralLedger.RecalculateBalances;

/// <summary>
/// Handler that recalculates account balances across the Chart of Accounts.
/// If an AccountId is provided the recalculation will run for that single account, otherwise for all accounts.
/// </summary>
public class RecalculateBalancesHandler(AccountingDbContext context) : ICommandHandler<RecalculateBalancesCommand>
{
    public async ValueTask<Unit> Handle(RecalculateBalancesCommand command, CancellationToken ct)
    {
        if (command.AccountId.HasValue)
        {
            var accountId = command.AccountId.Value;
            var sums = await context.JournalEntryLines
                .AsNoTracking()
                .Where(x => x.AccountId == accountId && x.IsActive)
                .GroupBy(x => x.AccountId)
                .Select(g => new { AccountId = g.Key, Debit = g.Sum(x => x.Debit), Credit = g.Sum(x => x.Credit) })
                .FirstOrDefaultAsync(ct);

            var newBalance = sums is null ? 0m : sums.Debit - sums.Credit;

            var account = await context.ChartOfAccounts.FirstOrDefaultAsync(x => x.Id == accountId, ct)
                ?? throw new NotFoundException("ChartOfAccount not found");

            account.UpdateBalance(newBalance);
            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }

        // Recalculate for all accounts
        var balances = await context.JournalEntryLines
            .AsNoTracking()
            .Where(x => x.IsActive)
            .GroupBy(x => x.AccountId)
            .Select(g => new { AccountId = g.Key, Debit = g.Sum(x => x.Debit), Credit = g.Sum(x => x.Credit) })
            .ToListAsync(ct);

        var accounts = await context.ChartOfAccounts.ToListAsync(ct);

        foreach (var acct in accounts)
        {
            var sums = balances.FirstOrDefault(b => b.AccountId == acct.Id);
            var newBalance = sums is null ? 0m : sums.Debit - sums.Credit;
            acct.UpdateBalance(newBalance);
        }

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}