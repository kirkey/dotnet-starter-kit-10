using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.InsuranceProducts.CreateInsuranceProduct;

namespace FSH.Module.Microfinance.Features.v1.InsuranceProducts.CreateInsuranceProduct;

public class CreateInsuranceProductHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateInsuranceProductCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInsuranceProductCommand command, CancellationToken ct)
    {
        var entity = InsuranceProduct.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.InsuranceProducts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
