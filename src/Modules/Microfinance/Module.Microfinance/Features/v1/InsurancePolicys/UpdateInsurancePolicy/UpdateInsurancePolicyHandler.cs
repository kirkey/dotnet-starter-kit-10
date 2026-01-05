using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InsurancePolicys.UpdateInsurancePolicy;

namespace FSH.Module.Microfinance.Features.v1.InsurancePolicys.UpdateInsurancePolicy;

public class UpdateInsurancePolicyHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateInsurancePolicyCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInsurancePolicyCommand command, CancellationToken ct)
    {
        var entity = await context.InsurancePolicys.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InsurancePolicy not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
