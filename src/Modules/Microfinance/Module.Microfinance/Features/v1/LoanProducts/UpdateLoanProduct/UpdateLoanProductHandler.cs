using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanProducts.UpdateLoanProduct;

namespace FSH.Module.Microfinance.Features.v1.LoanProducts.UpdateLoanProduct;

public class UpdateLoanProductHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanProductCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanProductCommand command, CancellationToken ct)
    {
        var entity = await context.LoanProducts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanProduct not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
