using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.AgentBankings;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.AgentBankings.GetAgentBanking;

public record GetAgentBankingQuery(Guid Id) : IQuery<AgentBankingDto>;

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
