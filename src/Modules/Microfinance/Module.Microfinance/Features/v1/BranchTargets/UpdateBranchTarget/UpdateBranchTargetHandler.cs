using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.BranchTargets.UpdateBranchTarget;

namespace FSH.Module.Microfinance.Features.v1.BranchTargets.UpdateBranchTarget;

public class UpdateBranchTargetHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateBranchTargetCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateBranchTargetCommand command, CancellationToken ct)
    {
        var entity = await context.BranchTargets.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("BranchTarget not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
