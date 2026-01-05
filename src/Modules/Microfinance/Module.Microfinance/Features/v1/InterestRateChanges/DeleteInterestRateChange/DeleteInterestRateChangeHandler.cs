using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.DeleteInterestRateChange;

namespace FSH.Module.Microfinance.Features.v1.InterestRateChanges.DeleteInterestRateChange;

public class DeleteInterestRateChangeHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteInterestRateChangeCommand>
{
    public async ValueTask<Unit> Handle(DeleteInterestRateChangeCommand command, CancellationToken ct)
    {
        var entity = await context.InterestRateChanges.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InterestRateChange not found");
        
        context.InterestRateChanges.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
