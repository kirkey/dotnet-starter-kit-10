using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.AgentBankings.CreateAgentBanking;

public record CreateAgentBankingCommand(string Name) : ICommand<Guid>;

public class CreateAgentBankingHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateAgentBankingCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateAgentBankingCommand command, CancellationToken ct)
    {
        var entity = AgentBanking.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.AgentBankings.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
