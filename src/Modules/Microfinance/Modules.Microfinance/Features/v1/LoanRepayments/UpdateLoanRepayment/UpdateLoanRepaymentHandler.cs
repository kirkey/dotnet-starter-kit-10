using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanRepayments.UpdateLoanRepayment;

public record UpdateLoanRepaymentCommand(Guid Id, string Name) : ICommand<Guid>;

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
