using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Accruals.UpdateAccrual;

public record UpdateAccrualCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateAccrualHandler(AccountingDbContext context) : ICommandHandler<UpdateAccrualCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateAccrualCommand command, CancellationToken ct)
    {
        var entity = await context.Accruals.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Accrual not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
