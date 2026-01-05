using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.BudgetDetails.DeleteBudgetDetail;

namespace FSH.Module.Accounting.Features.v1.BudgetDetails.DeleteBudgetDetail; 

public class DeleteBudgetDetailHandler(AccountingDbContext context) : ICommandHandler<DeleteBudgetDetailCommand>
{
    public async ValueTask<Unit> Handle(DeleteBudgetDetailCommand command, CancellationToken ct)
    {
        var entity = await context.BudgetDetails.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("BudgetDetail not found");
        
        context.BudgetDetails.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
