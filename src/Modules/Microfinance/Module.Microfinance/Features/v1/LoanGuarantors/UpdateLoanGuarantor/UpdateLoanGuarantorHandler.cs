using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanGuarantors.UpdateLoanGuarantor;

namespace FSH.Module.Microfinance.Features.v1.LoanGuarantors.UpdateLoanGuarantor;

public class UpdateLoanGuarantorHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanGuarantorCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanGuarantorCommand command, CancellationToken ct)
    {
        var entity = await context.LoanGuarantors.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanGuarantor not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
