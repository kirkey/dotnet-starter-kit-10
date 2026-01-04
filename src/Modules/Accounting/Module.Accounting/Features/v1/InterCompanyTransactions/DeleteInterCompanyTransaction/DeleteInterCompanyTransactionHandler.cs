using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.InterCompanyTransactions.DeleteInterCompanyTransaction;

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
