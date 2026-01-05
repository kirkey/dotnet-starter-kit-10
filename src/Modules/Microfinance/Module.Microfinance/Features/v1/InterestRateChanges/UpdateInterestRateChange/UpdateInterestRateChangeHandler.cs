using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.UpdateInterestRateChange;

namespace FSH.Module.Microfinance.Features.v1.InterestRateChanges.UpdateInterestRateChange;

public class UpdateInterestRateChangeHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateInterestRateChangeCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInterestRateChangeCommand command, CancellationToken ct)
    {
        var entity = await context.InterestRateChanges.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InterestRateChange not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
