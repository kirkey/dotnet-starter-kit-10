using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.StaffTrainings.UpdateStaffTraining;

namespace FSH.Module.Microfinance.Features.v1.StaffTrainings.UpdateStaffTraining;

public class UpdateStaffTrainingHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateStaffTrainingCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateStaffTrainingCommand command, CancellationToken ct)
    {
        var entity = await context.StaffTrainings.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("StaffTraining not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
