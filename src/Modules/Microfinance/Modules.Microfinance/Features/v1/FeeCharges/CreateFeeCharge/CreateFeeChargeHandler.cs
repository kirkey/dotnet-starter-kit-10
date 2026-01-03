using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.FeeCharges.CreateFeeCharge;

public record CreateFeeChargeCommand(string Name) : ICommand<Guid>;

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
