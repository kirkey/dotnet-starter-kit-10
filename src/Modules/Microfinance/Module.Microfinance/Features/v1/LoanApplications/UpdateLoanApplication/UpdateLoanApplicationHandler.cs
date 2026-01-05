using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanApplications;namespace FSH.Module.Microfinance.Features.v1.LoanApplications.UpdateLoanApplication;

public class UpdateLoanApplicationHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanApplicationCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanApplicationCommand command, CancellationToken ct)
    {
        var entity = await context.LoanApplications.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanApplication not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
