using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanSchedules.DeleteLoanSchedule;

public record DeleteLoanScheduleCommand(Guid Id) : ICommand;

public class DeleteLoanScheduleHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanScheduleCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanScheduleCommand command, CancellationToken ct)
    {
        var entity = await context.LoanSchedules.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanSchedule not found");
        
        context.LoanSchedules.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
