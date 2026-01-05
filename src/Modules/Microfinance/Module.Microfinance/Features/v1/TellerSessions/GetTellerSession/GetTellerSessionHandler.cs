using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.TellerSessions.GetTellerSession;
using FSH.Module.Microfinance.Contracts.v1.TellerSessions;

namespace FSH.Module.Microfinance.Features.v1.TellerSessions.GetTellerSession;

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
