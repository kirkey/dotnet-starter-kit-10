using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.StaffTrainings.UpdateStaffTraining;

public record UpdateStaffTrainingCommand(Guid Id, string Name) : ICommand<Guid>;

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
