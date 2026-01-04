using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.PrepaidExpenses.DeletePrepaidExpense;

public record DeletePrepaidExpenseCommand(Guid Id) : ICommand;

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
