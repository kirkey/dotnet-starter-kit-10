using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.ProjectCosts.DeleteProjectCost;

public record DeleteProjectCostCommand(Guid Id) : ICommand;

public class DeleteProjectCostHandler(AccountingDbContext context) : ICommandHandler<DeleteProjectCostCommand>
{
    public async ValueTask<Unit> Handle(DeleteProjectCostCommand command, CancellationToken ct)
    {
        var entity = await context.ProjectCosts.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("ProjectCost not found");
        
        context.ProjectCosts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
