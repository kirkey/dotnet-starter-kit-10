using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanSchedules.UpdateLoanSchedule;

namespace FSH.Module.Microfinance.Features.v1.LoanSchedules.UpdateLoanSchedule;

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
