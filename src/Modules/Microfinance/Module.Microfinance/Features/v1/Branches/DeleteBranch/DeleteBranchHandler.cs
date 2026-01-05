using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.Branches.DeleteBranch;

namespace FSH.Module.Microfinance.Features.v1.Branches.DeleteBranch;

public class DeleteBranchHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteBranchCommand>
{
    public async ValueTask<Unit> Handle(DeleteBranchCommand command, CancellationToken ct)
    {
        var entity = await context.Branches.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("Branch not found");
        
        context.Branches.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
