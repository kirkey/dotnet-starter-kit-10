using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.BudgetDetails.UpdateBudgetDetail;

public record UpdateBudgetDetailCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateBudgetDetailHandler(AccountingDbContext context) : ICommandHandler<UpdateBudgetDetailCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateBudgetDetailCommand command, CancellationToken ct)
    {
        var entity = await context.BudgetDetails.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("BudgetDetail not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
