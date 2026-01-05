using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanRepayments.DeleteLoanRepayment;

namespace FSH.Module.Microfinance.Features.v1.LoanRepayments.DeleteLoanRepayment;

public class DeleteLoanRepaymentHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanRepaymentCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanRepaymentCommand command, CancellationToken ct)
    {
        var entity = await context.LoanRepayments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanRepayment not found");
        
        context.LoanRepayments.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
