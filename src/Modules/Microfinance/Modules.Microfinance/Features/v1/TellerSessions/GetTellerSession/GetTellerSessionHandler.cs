using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.TellerSessions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.TellerSessions.GetTellerSession;

public record GetTellerSessionQuery(Guid Id) : IQuery<TellerSessionDto>;

public class GetTellerSessionHandler(MicrofinanceDbContext context) : IQueryHandler<GetTellerSessionQuery, TellerSessionDto>
{
    public async ValueTask<TellerSessionDto> Handle(GetTellerSessionQuery query, CancellationToken ct)
    {
        var entity = await context.TellerSessions
            .Where(x => x.Id == query.Id)
            .Select(x => new TellerSessionDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("TellerSession not found");
    }
}
