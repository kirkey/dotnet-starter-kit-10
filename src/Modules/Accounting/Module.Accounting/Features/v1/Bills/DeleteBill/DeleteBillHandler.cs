using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Bills.DeleteBill;

public record DeleteBillCommand(Guid Id) : ICommand;

public class DeleteBillHandler(AccountingDbContext context) : ICommandHandler<DeleteBillCommand>
{
    public async ValueTask<Unit> Handle(DeleteBillCommand command, CancellationToken ct)
    {
        var entity = await context.Bills.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Bill not found");
        
        context.Bills.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
