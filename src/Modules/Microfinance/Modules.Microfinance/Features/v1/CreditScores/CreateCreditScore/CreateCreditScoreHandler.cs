using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.CreditScores.CreateCreditScore;

public record CreateCreditScoreCommand(string Name) : ICommand<Guid>;

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
