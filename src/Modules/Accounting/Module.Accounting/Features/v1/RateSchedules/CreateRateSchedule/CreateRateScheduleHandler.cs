using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.RateSchedules.CreateRateSchedule;

namespace FSH.Module.Accounting.Features.v1.RateSchedules.CreateRateSchedule; 

public class CreateRateScheduleHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateRateScheduleCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateRateScheduleCommand command, CancellationToken ct)
    {
        var entity = RateSchedule.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.RateSchedules.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
