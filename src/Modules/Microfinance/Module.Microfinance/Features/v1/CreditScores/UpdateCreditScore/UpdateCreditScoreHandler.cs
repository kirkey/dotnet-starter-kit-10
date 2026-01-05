using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CreditScores.UpdateCreditScore;

namespace FSH.Module.Microfinance.Features.v1.CreditScores.UpdateCreditScore;

public class UpdateCreditScoreHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCreditScoreCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCreditScoreCommand command, CancellationToken ct)
    {
        var entity = await context.CreditScores.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CreditScore not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
