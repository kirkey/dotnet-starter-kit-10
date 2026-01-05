using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.FeeWaivers.CreateFeeWaiver;

namespace FSH.Module.Microfinance.Features.v1.FeeWaivers.CreateFeeWaiver;

public class CreateFeeWaiverHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateFeeWaiverCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateFeeWaiverCommand command, CancellationToken ct)
    {
        var entity = FeeWaiver.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.FeeWaivers.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
