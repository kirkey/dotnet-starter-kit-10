using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.InterestRateChanges.CreateInterestRateChange;

public record CreateInterestRateChangeCommand(string Name) : ICommand<Guid>;

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
