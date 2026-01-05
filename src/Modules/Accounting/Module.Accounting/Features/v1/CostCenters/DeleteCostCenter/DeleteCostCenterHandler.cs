using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.CostCenters.DeleteCostCenter;namespace FSH.Module.Accounting.Features.v1.CostCenters.DeleteCostCenter;

public class DeleteCostCenterHandler(AccountingDbContext context) : ICommandHandler<DeleteCostCenterCommand>
{
    public async ValueTask<Unit> Handle(DeleteCostCenterCommand command, CancellationToken ct)
    {
        var entity = await context.CostCenters.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("CostCenter not found");

        // Business rule: prevent deleting cost centers that have dependent records
        var hasChildren = await context.CostCenters.AnyAsync(c => c.ParentCostCenterId == command.Id, ct).ConfigureAwait(false);
        var hasJournalLines = await context.JournalEntryLines.AnyAsync(x => x.CostCenterId == command.Id, ct).ConfigureAwait(false);
        var hasPrepaidExpenses = await context.PrepaidExpenses.AnyAsync(x => x.CostCenterId == command.Id, ct).ConfigureAwait(false);
        var hasProjectCosts = await context.ProjectCosts.AnyAsync(x => x.CostCenterId == command.Id, ct).ConfigureAwait(false);
        var hasBillLineItems = await context.BillLineItems.AnyAsync(x => x.CostCenterId == command.Id, ct).ConfigureAwait(false);

        if (hasChildren || hasJournalLines || hasPrepaidExpenses || hasProjectCosts || hasBillLineItems)
            throw new BadRequestException("Cannot delete cost center with dependent records. Remove or reassign dependent records first.");
        
        context.CostCenters.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
} 
