using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations.RemoveBankReconciliationLine;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.RemoveBankReconciliationLine;

/// <summary>
/// Handler for removing a line from a BankReconciliation. Operation is idempotent.
/// </summary>
public class RemoveBankReconciliationLineHandler(AccountingDbContext context) : ICommandHandler<RemoveBankReconciliationLineCommand>
{
    public async ValueTask<Unit> Handle(RemoveBankReconciliationLineCommand command, CancellationToken ct)
    {
        var entity = await context.BankReconciliationLines.FirstOrDefaultAsync(x => x.Id == command.BankReconciliationLineId, ct);
        if (entity is null)
            return Unit.Value; // Idempotent: nothing to remove

        context.BankReconciliationLines.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}