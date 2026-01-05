using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InsurancePolicys.DeleteInsurancePolicy;

namespace FSH.Module.Microfinance.Features.v1.InsurancePolicys.DeleteInsurancePolicy;

public class DeleteInsurancePolicyHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteInsurancePolicyCommand>
{
    public async ValueTask<Unit> Handle(DeleteInsurancePolicyCommand command, CancellationToken ct)
    {
        var entity = await context.InsurancePolicys.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InsurancePolicy not found");
        
        context.InsurancePolicys.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
