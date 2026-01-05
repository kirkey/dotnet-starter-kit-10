using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanRepayments.UpdateLoanRepayment;

namespace FSH.Module.Microfinance.Features.v1.LoanRepayments.UpdateLoanRepayment;

public class UpdateLoanRepaymentHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanRepaymentCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanRepaymentCommand command, CancellationToken ct)
    {
        var entity = await context.LoanRepayments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanRepayment not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
