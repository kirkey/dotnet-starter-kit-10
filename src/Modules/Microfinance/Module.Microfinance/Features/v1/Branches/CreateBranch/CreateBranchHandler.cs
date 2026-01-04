using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.Branches.CreateBranch;

public record CreateBranchCommand(string Name) : ICommand<Guid>;

public class CreateBranchHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateBranchCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBranchCommand command, CancellationToken ct)
    {
        var entity = Branch.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.Branches.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
