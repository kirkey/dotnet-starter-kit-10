using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanOfficerTargets.DeleteLoanOfficerTarget;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerTargets.DeleteLoanOfficerTarget;

public class DeleteLoanOfficerTargetHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanOfficerTargetCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanOfficerTargetCommand command, CancellationToken ct)
    {
        var entity = await context.LoanOfficerTargets.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanOfficerTarget not found");
        
        context.LoanOfficerTargets.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
