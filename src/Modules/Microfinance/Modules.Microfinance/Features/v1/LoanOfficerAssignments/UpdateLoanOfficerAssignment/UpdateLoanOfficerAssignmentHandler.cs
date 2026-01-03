using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanOfficerAssignments.UpdateLoanOfficerAssignment;

public record UpdateLoanOfficerAssignmentCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateLoanOfficerAssignmentHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanOfficerAssignmentCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanOfficerAssignmentCommand command, CancellationToken ct)
    {
        var entity = await context.LoanOfficerAssignments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanOfficerAssignment not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
