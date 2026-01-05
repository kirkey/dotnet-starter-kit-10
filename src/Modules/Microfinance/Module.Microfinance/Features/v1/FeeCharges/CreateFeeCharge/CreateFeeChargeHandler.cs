using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.FeeCharges.CreateFeeCharge;

namespace FSH.Module.Microfinance.Features.v1.FeeCharges.CreateFeeCharge;

public class CreateFeeChargeHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateFeeChargeCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateFeeChargeCommand command, CancellationToken ct)
    {
        var entity = FeeCharge.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.FeeCharges.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
