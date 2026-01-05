using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions.UpdateInterCompanyTransaction;

namespace FSH.Module.Accounting.Features.v1.InterCompanyTransactions.UpdateInterCompanyTransaction;

public class UpdateInterCompanyTransactionHandler(AccountingDbContext context) : ICommandHandler<UpdateInterCompanyTransactionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInterCompanyTransactionCommand command, CancellationToken ct)
    {
        var entity = await context.InterCompanyTransactions.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("InterCompanyTransaction not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
