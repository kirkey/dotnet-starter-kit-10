using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.CreditScores.CreateCreditScore;

namespace FSH.Module.Microfinance.Features.v1.CreditScores.CreateCreditScore;

public class CreateCreditScoreHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCreditScoreCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCreditScoreCommand command, CancellationToken ct)
    {
        var entity = CreditScore.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CreditScores.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
