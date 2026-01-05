using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.DeferredRevenue.DeleteDeferredRevenue;namespace FSH.Module.Accounting.Features.v1.DeferredRevenue.DeleteDeferredRevenue;

public class DeleteDeferredRevenueHandler(AccountingDbContext context) : ICommandHandler<DeleteDeferredRevenueCommand>
{
    public async ValueTask<Unit> Handle(DeleteDeferredRevenueCommand command, CancellationToken ct)
    {
        var entity = await context.DeferredRevenue.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("DeferredRevenue not found");
        
        context.DeferredRevenue.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
