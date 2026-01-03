using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.Branches.UpdateBranch;

public record UpdateBranchCommand(Guid Id, string Name) : ICommand<Guid>;

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
