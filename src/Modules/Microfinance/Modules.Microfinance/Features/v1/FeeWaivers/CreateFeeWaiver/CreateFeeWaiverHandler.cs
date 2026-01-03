using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.FeeWaivers.CreateFeeWaiver;

public record CreateFeeWaiverCommand(string Name) : ICommand<Guid>;

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
