using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Consumption.UpdateConsumption;

public record UpdateConsumptionCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

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
