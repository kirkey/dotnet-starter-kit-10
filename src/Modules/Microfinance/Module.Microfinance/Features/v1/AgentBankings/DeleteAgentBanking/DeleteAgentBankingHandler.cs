using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.AgentBankings.DeleteAgentBanking;

public record DeleteAgentBankingCommand(Guid Id) : ICommand;

public class DeleteAgentBankingHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteAgentBankingCommand>
{
    public async ValueTask<Unit> Handle(DeleteAgentBankingCommand command, CancellationToken ct)
    {
        var entity = await context.AgentBankings.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("AgentBanking not found");
        
        context.AgentBankings.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
