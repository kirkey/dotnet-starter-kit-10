using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.UssdSessions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.UssdSessions.GetUssdSession;

public record GetUssdSessionQuery(Guid Id) : IQuery<UssdSessionDto>;

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
