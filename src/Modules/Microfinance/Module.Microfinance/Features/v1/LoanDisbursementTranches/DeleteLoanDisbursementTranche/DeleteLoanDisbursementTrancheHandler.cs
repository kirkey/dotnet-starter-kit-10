using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanDisbursementTranches.DeleteLoanDisbursementTranche;

public record DeleteLoanDisbursementTrancheCommand(Guid Id) : ICommand;

public class DeleteLoanDisbursementTrancheHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanDisbursementTrancheCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanDisbursementTrancheCommand command, CancellationToken ct)
    {
        var entity = await context.LoanDisbursementTranches.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanDisbursementTranche not found");
        
        context.LoanDisbursementTranches.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
