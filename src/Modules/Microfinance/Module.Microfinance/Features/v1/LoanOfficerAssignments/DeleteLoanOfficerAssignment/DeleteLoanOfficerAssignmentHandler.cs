using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerAssignments.DeleteLoanOfficerAssignment;

public record DeleteLoanOfficerAssignmentCommand(Guid Id) : ICommand;

public class DeleteLoanOfficerAssignmentHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanOfficerAssignmentCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanOfficerAssignmentCommand command, CancellationToken ct)
    {
        var entity = await context.LoanOfficerAssignments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanOfficerAssignment not found");
        
        context.LoanOfficerAssignments.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
