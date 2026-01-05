using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.DeferredRevenue.UpdateDeferredRevenue;namespace FSH.Module.Accounting.Features.v1.DeferredRevenue.UpdateDeferredRevenue;

public class UpdateDeferredRevenueHandler(AccountingDbContext context) : ICommandHandler<UpdateDeferredRevenueCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateDeferredRevenueCommand command, CancellationToken ct)
    {
        var entity = await context.DeferredRevenue.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("DeferredRevenue not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
