using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches.UpdateLoanDisbursementTranche;

namespace FSH.Module.Microfinance.Features.v1.LoanDisbursementTranches.UpdateLoanDisbursementTranche;

public class UpdateLoanDisbursementTrancheHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanDisbursementTrancheCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanDisbursementTrancheCommand command, CancellationToken ct)
    {
        var entity = await context.LoanDisbursementTranches.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanDisbursementTranche not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
