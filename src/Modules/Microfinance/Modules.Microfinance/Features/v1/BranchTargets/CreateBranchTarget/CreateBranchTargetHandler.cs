using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.BranchTargets.CreateBranchTarget;

public record CreateBranchTargetCommand(string Name) : ICommand<Guid>;

public class CreateBranchTargetHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateBranchTargetCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBranchTargetCommand command, CancellationToken ct)
    {
        var entity = BranchTarget.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.BranchTargets.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
