using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Payees.UpdatePayee;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Payees.UpdatePayee;

public class UpdatePayeeHandler(AccountingDbContext context) : ICommandHandler<UpdatePayeeCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdatePayeeCommand command, CancellationToken ct)
    {
        var entity = await context.Payees.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Payee not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
