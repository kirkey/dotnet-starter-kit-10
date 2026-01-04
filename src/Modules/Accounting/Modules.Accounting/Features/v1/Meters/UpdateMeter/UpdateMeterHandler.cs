using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Meters.UpdateMeter;

public record UpdateMeterCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateMeterHandler(AccountingDbContext context) : ICommandHandler<UpdateMeterCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateMeterCommand command, CancellationToken ct)
    {
        var entity = await context.Meters.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Meter not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
