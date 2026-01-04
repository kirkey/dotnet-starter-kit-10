using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.CostCenters.DeleteCostCenter;

public record DeleteCostCenterCommand(Guid Id) : ICommand;

public class DeleteCostCenterHandler(AccountingDbContext context) : ICommandHandler<DeleteCostCenterCommand>
{
    public async ValueTask<Unit> Handle(DeleteCostCenterCommand command, CancellationToken ct)
    {
        var entity = await context.CostCenters.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("CostCenter not found");
        
        context.CostCenters.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
