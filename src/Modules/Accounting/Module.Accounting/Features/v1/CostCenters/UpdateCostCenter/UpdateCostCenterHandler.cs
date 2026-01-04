using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.CostCenters.UpdateCostCenter;

public record UpdateCostCenterCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateCostCenterHandler(AccountingDbContext context) : ICommandHandler<UpdateCostCenterCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCostCenterCommand command, CancellationToken ct)
    {
        var entity = await context.CostCenters.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("CostCenter not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
