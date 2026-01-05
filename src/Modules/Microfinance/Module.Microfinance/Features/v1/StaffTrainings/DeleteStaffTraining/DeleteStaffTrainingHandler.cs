using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.StaffTrainings.DeleteStaffTraining;

namespace FSH.Module.Microfinance.Features.v1.StaffTrainings.DeleteStaffTraining;

public class DeleteStaffTrainingHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteStaffTrainingCommand>
{
    public async ValueTask<Unit> Handle(DeleteStaffTrainingCommand command, CancellationToken ct)
    {
        var entity = await context.StaffTrainings.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("StaffTraining not found");
        
        context.StaffTrainings.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
