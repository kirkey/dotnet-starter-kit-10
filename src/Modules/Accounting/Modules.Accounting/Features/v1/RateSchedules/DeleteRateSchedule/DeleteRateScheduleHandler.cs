using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.RateSchedules.DeleteRateSchedule;

public record DeleteRateScheduleCommand(Guid Id) : ICommand;

public class DeleteRateScheduleHandler(AccountingDbContext context) : ICommandHandler<DeleteRateScheduleCommand>
{
    public async ValueTask<Unit> Handle(DeleteRateScheduleCommand command, CancellationToken ct)
    {
        var entity = await context.RateSchedules.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("RateSchedule not found");
        
        context.RateSchedules.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
