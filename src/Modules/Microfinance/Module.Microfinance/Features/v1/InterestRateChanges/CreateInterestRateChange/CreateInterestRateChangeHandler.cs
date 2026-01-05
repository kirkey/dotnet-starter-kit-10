using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.CreateInterestRateChange;

namespace FSH.Module.Microfinance.Features.v1.InterestRateChanges.CreateInterestRateChange;

public class CreateInterestRateChangeHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateInterestRateChangeCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInterestRateChangeCommand command, CancellationToken ct)
    {
        var entity = InterestRateChange.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.InterestRateChanges.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
