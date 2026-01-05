using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.Branches.UpdateBranch;

namespace FSH.Module.Microfinance.Features.v1.Branches.UpdateBranch;

public class UpdateBranchHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateBranchCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateBranchCommand command, CancellationToken ct)
    {
        var entity = await context.Branches.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("Branch not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
