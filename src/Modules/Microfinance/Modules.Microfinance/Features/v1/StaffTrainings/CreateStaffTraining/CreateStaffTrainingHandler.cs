using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.StaffTrainings.CreateStaffTraining;

public record CreateStaffTrainingCommand(string Name) : ICommand<Guid>;

public class CreateStaffTrainingHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateStaffTrainingCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateStaffTrainingCommand command, CancellationToken ct)
    {
        var entity = StaffTraining.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.StaffTrainings.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
