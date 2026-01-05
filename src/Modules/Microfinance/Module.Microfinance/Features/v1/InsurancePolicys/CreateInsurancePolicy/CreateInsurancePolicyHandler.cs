using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.InsurancePolicys.CreateInsurancePolicy;

namespace FSH.Module.Microfinance.Features.v1.InsurancePolicys.CreateInsurancePolicy;

public class CreateInsurancePolicyHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateInsurancePolicyCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInsurancePolicyCommand command, CancellationToken ct)
    {
        var entity = InsurancePolicy.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.InsurancePolicys.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
