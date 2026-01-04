using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.AccountingPeriods.DeleteAccountingPeriod;

public record DeleteAccountingPeriodCommand(Guid Id) : ICommand;

public class DeleteAccountingPeriodHandler(AccountingDbContext context) : ICommandHandler<DeleteAccountingPeriodCommand>
{
    public async ValueTask<Unit> Handle(DeleteAccountingPeriodCommand command, CancellationToken ct)
    {
        var entity = await context.AccountingPeriods.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("AccountingPeriod not found");

        // Business rule: prevent deleting accounting periods that have dependent records
        var hasJournalEntries = await context.JournalEntries.AnyAsync(j => j.FiscalPeriodId == command.Id, ct).ConfigureAwait(false);
        var hasAccountReconciliations = await context.AccountReconciliations.AnyAsync(a => a.AccountingPeriodId == command.Id, ct).ConfigureAwait(false);
        var hasFiscalPeriodCloses = await context.FiscalPeriodClose.AnyAsync(f => f.FiscalPeriodId == command.Id, ct).ConfigureAwait(false);

        if (hasJournalEntries || hasAccountReconciliations || hasFiscalPeriodCloses)
            throw new BadRequestException("Cannot delete accounting period with dependent records. Remove or reassign dependent records first.");

        context.AccountingPeriods.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
