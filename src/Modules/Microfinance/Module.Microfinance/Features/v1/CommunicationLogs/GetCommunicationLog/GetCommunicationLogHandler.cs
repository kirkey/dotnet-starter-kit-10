using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.GetCommunicationLog;
using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs;

namespace FSH.Module.Microfinance.Features.v1.CommunicationLogs.GetCommunicationLog;

public class GetCommunicationLogHandler(MicrofinanceDbContext context) : IQueryHandler<GetCommunicationLogQuery, CommunicationLogDto>
{
    public async ValueTask<CommunicationLogDto> Handle(GetCommunicationLogQuery query, CancellationToken ct)
    {
        var entity = await context.CommunicationLogs
            .Where(x => x.Id == query.Id)
            .Select(x => new CommunicationLogDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CommunicationLog not found");
    }
}
