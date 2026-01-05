using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.DeletePrepaidExpense;

namespace FSH.Module.Accounting.Features.v1.PrepaidExpenses.DeletePrepaidExpense;

public class DeletePrepaidExpenseHandler(AccountingDbContext context) : ICommandHandler<DeletePrepaidExpenseCommand>
{
    public async ValueTask<Unit> Handle(DeletePrepaidExpenseCommand command, CancellationToken ct)
    {
        var entity = await context.PrepaidExpenses.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("PrepaidExpense not found");
        
        context.PrepaidExpenses.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
