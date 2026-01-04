using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanDisbursementTranches.UpdateLoanDisbursementTranche;

public record UpdateLoanDisbursementTrancheCommand(Guid Id, string Name) : ICommand<Guid>;

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
