using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanOfficerTargets.UpdateLoanOfficerTarget;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerTargets.UpdateLoanOfficerTarget;

public class UpdateLoanOfficerTargetHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanOfficerTargetCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanOfficerTargetCommand command, CancellationToken ct)
    {
        var entity = await context.LoanOfficerTargets.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanOfficerTarget not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
