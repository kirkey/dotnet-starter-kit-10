using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.StaffTrainings.CreateStaffTraining;

namespace FSH.Module.Microfinance.Features.v1.StaffTrainings.CreateStaffTraining;

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
