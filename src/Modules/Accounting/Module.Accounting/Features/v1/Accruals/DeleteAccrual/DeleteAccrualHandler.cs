using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Accruals.DeleteAccrual;

public record DeleteAccrualCommand(Guid Id) : ICommand;

public class DeleteAccrualHandler(AccountingDbContext context) : ICommandHandler<DeleteAccrualCommand>
{
    public async ValueTask<Unit> Handle(DeleteAccrualCommand command, CancellationToken ct)
    {
        var entity = await context.Accruals.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Accrual not found");
        
        context.Accruals.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
