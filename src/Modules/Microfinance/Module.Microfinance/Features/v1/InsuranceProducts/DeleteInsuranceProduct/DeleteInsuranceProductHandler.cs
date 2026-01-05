using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InsuranceProducts.DeleteInsuranceProduct;

namespace FSH.Module.Microfinance.Features.v1.InsuranceProducts.DeleteInsuranceProduct;

public class DeleteInsuranceProductHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteInsuranceProductCommand>
{
    public async ValueTask<Unit> Handle(DeleteInsuranceProductCommand command, CancellationToken ct)
    {
        var entity = await context.InsuranceProducts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InsuranceProduct not found");
        
        context.InsuranceProducts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
