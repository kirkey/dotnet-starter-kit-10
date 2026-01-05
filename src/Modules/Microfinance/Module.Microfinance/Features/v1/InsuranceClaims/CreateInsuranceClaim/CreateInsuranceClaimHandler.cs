using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.CreateInsuranceClaim;

namespace FSH.Module.Microfinance.Features.v1.InsuranceClaims.CreateInsuranceClaim;

public class CreateInsuranceClaimHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateInsuranceClaimCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInsuranceClaimCommand command, CancellationToken ct)
    {
        var entity = InsuranceClaim.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.InsuranceClaims.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
