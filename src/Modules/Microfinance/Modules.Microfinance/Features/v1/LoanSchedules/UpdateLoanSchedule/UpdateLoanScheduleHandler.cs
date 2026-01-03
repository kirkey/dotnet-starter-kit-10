using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanSchedules.UpdateLoanSchedule;

public record UpdateLoanScheduleCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateLoanScheduleHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanScheduleCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanScheduleCommand command, CancellationToken ct)
    {
        var entity = await context.LoanSchedules.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanSchedule not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
