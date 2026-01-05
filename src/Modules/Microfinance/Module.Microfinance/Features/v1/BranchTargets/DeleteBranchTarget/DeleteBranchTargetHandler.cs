using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.BranchTargets.DeleteBranchTarget;

namespace FSH.Module.Microfinance.Features.v1.BranchTargets.DeleteBranchTarget;

public class DeleteBranchTargetHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteBranchTargetCommand>
{
    public async ValueTask<Unit> Handle(DeleteBranchTargetCommand command, CancellationToken ct)
    {
        var entity = await context.BranchTargets.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("BranchTarget not found");
        
        context.BranchTargets.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
