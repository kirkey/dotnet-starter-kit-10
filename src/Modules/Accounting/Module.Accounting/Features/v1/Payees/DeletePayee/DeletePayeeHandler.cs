using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Payees.DeletePayee;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Payees.DeletePayee;

public class DeletePayeeHandler(AccountingDbContext context) : ICommandHandler<DeletePayeeCommand>
{
    public async ValueTask<Unit> Handle(DeletePayeeCommand command, CancellationToken ct)
    {
        var entity = await context.Payees.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Payee not found");
        
        context.Payees.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
