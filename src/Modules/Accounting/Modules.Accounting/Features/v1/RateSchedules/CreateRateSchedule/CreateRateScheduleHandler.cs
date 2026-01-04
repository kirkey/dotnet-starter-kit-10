using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.RateSchedules.CreateRateSchedule;

public record CreateRateScheduleCommand(string Name, string? Description) : ICommand<Guid>;

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
