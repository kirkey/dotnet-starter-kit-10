using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanGuarantors.DeleteLoanGuarantor;

public record DeleteLoanGuarantorCommand(Guid Id) : ICommand;

public class DeleteLoanGuarantorHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanGuarantorCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanGuarantorCommand command, CancellationToken ct)
    {
        var entity = await context.LoanGuarantors.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanGuarantor not found");
        
        context.LoanGuarantors.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
