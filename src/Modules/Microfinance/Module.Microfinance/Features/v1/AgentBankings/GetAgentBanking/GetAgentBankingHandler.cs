using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.AgentBankings;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.AgentBankings.GetAgentBanking;

namespace FSH.Module.Microfinance.Features.v1.AgentBankings.GetAgentBanking;

public class GetAgentBankingHandler(MicrofinanceDbContext context) : IQueryHandler<GetAgentBankingQuery, AgentBankingDto>
{
    public async ValueTask<AgentBankingDto> Handle(GetAgentBankingQuery query, CancellationToken ct)
    {
        var entity = await context.AgentBankings
            .Where(x => x.Id == query.Id)
            .Select(x => new AgentBankingDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("AgentBanking not found");
    }
}
