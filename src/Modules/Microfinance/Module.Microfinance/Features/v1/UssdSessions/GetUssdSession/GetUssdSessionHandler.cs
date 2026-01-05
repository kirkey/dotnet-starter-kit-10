using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.UssdSessions.GetUssdSession;
using FSH.Module.Microfinance.Contracts.v1.UssdSessions;

namespace FSH.Module.Microfinance.Features.v1.UssdSessions.GetUssdSession;

public class GetUssdSessionHandler(MicrofinanceDbContext context) : IQueryHandler<GetUssdSessionQuery, UssdSessionDto>
{
    public async ValueTask<UssdSessionDto> Handle(GetUssdSessionQuery query, CancellationToken ct)
    {
        var entity = await context.UssdSessions
            .Where(x => x.Id == query.Id)
            .Select(x => new UssdSessionDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("UssdSession not found");
    }
}
