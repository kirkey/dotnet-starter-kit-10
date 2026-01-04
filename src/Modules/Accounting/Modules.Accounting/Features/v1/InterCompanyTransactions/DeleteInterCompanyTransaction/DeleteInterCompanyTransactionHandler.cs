using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.InterCompanyTransactions.DeleteInterCompanyTransaction;

public record DeleteInterCompanyTransactionCommand(Guid Id) : ICommand;

public class DeleteInterCompanyTransactionHandler(AccountingDbContext context) : ICommandHandler<DeleteInterCompanyTransactionCommand>
{
    public async ValueTask<Unit> Handle(DeleteInterCompanyTransactionCommand command, CancellationToken ct)
    {
        var entity = await context.InterCompanyTransactions.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("InterCompanyTransaction not found");
        
        context.InterCompanyTransactions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
