using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.DeferredRevenue.DeleteDeferredRevenue;

public record DeleteDeferredRevenueCommand(Guid Id) : ICommand;

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
