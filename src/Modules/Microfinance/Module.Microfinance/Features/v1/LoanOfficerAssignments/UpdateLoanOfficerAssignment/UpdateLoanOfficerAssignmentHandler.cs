using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments.UpdateLoanOfficerAssignment;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerAssignments.UpdateLoanOfficerAssignment;

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
