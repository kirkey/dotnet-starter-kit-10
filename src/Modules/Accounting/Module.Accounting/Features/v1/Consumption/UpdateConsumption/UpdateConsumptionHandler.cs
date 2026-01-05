using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.Consumption.UpdateConsumption;namespace FSH.Module.Accounting.Features.v1.Consumption.UpdateConsumption;

public class UpdateConsumptionHandler(AccountingDbContext context) : ICommandHandler<UpdateConsumptionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateConsumptionCommand command, CancellationToken ct)
    {
        var entity = await context.Consumption.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Consumption not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
