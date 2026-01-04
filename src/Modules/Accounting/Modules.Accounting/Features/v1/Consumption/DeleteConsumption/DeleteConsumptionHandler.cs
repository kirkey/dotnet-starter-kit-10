using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Consumption.DeleteConsumption;

public record DeleteConsumptionCommand(Guid Id) : ICommand;

public class DeleteConsumptionHandler(AccountingDbContext context) : ICommandHandler<DeleteConsumptionCommand>
{
    public async ValueTask<Unit> Handle(DeleteConsumptionCommand command, CancellationToken ct)
    {
        var entity = await context.Consumption.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Consumption not found");
        
        context.Consumption.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
