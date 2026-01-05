using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InsuranceProducts.UpdateInsuranceProduct;

namespace FSH.Module.Microfinance.Features.v1.InsuranceProducts.UpdateInsuranceProduct;

public class UpdateInsuranceProductHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateInsuranceProductCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInsuranceProductCommand command, CancellationToken ct)
    {
        var entity = await context.InsuranceProducts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InsuranceProduct not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
