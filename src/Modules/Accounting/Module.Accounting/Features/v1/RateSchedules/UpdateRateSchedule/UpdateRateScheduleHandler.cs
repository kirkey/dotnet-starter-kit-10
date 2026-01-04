using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.RateSchedules.UpdateRateSchedule;

public record UpdateRateScheduleCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateRateScheduleHandler(AccountingDbContext context) : ICommandHandler<UpdateRateScheduleCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateRateScheduleCommand command, CancellationToken ct)
    {
        var entity = await context.RateSchedules.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("RateSchedule not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
