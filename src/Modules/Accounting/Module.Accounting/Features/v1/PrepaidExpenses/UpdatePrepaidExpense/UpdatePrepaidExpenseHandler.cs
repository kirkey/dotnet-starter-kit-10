using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.UpdatePrepaidExpense;

namespace FSH.Module.Accounting.Features.v1.PrepaidExpenses.UpdatePrepaidExpense;

public class UpdatePrepaidExpenseHandler(AccountingDbContext context) : ICommandHandler<UpdatePrepaidExpenseCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdatePrepaidExpenseCommand command, CancellationToken ct)
    {
        var entity = await context.PrepaidExpenses.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("PrepaidExpense not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
