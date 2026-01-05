using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanApplications;namespace FSH.Module.Microfinance.Features.v1.LoanApplications.DeleteLoanApplication;

public class DeleteLoanApplicationHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanApplicationCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanApplicationCommand command, CancellationToken ct)
    {
        var entity = await context.LoanApplications.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanApplication not found");
        
        context.LoanApplications.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
