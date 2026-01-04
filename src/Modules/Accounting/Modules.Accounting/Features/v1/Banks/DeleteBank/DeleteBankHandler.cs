using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Banks.DeleteBank;

public record DeleteBankCommand(Guid Id) : ICommand;

public class DeleteBankHandler(AccountingDbContext context) : ICommandHandler<DeleteBankCommand>
{
    public async ValueTask<Unit> Handle(DeleteBankCommand command, CancellationToken ct)
    {
        var entity = await context.Banks.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Bank not found");
        
        context.Banks.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
