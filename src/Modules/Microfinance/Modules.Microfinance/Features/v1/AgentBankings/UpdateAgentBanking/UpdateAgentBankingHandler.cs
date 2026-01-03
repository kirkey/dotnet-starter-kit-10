using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.AgentBankings.UpdateAgentBanking;

public record UpdateAgentBankingCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateAgentBankingHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateAgentBankingCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateAgentBankingCommand command, CancellationToken ct)
    {
        var entity = await context.AgentBankings.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("AgentBanking not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
