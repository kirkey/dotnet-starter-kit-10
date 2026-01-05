using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CreditScores.DeleteCreditScore;

namespace FSH.Module.Microfinance.Features.v1.CreditScores.DeleteCreditScore;

public class DeleteCreditScoreHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCreditScoreCommand>
{
    public async ValueTask<Unit> Handle(DeleteCreditScoreCommand command, CancellationToken ct)
    {
        var entity = await context.CreditScores.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CreditScore not found");
        
        context.CreditScores.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
