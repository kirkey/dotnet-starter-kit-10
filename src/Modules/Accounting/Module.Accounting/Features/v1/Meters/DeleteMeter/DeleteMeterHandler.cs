using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Meters.DeleteMeter;

public record DeleteMeterCommand(Guid Id) : ICommand;

public class DeleteMeterHandler(AccountingDbContext context) : ICommandHandler<DeleteMeterCommand>
{
    public async ValueTask<Unit> Handle(DeleteMeterCommand command, CancellationToken ct)
    {
        var entity = await context.Meters.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Meter not found");
        
        context.Meters.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
