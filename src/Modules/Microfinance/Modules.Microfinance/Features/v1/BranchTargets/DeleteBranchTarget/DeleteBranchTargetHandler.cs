using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.BranchTargets.DeleteBranchTarget;

public record DeleteBranchTargetCommand(Guid Id) : ICommand;

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
