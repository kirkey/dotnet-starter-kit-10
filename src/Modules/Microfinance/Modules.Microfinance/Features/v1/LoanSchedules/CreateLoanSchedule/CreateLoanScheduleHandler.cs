using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.LoanSchedules.CreateLoanSchedule;

public record CreateLoanScheduleCommand(string Name) : ICommand<Guid>;

public class CreateLoanScheduleHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLoanScheduleCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLoanScheduleCommand command, CancellationToken ct)
    {
        var entity = LoanSchedule.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.LoanSchedules.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
