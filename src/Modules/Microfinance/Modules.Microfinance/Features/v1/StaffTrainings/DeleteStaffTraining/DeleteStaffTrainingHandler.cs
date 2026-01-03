using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.StaffTrainings.DeleteStaffTraining;

public record DeleteStaffTrainingCommand(Guid Id) : ICommand;

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
