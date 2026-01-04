using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CommunicationTemplates.GetCommunicationTemplate;

public record GetCommunicationTemplateQuery(Guid Id) : IQuery<CommunicationTemplateDto>;

public class GetCommunicationTemplateHandler(MicrofinanceDbContext context) : IQueryHandler<GetCommunicationTemplateQuery, CommunicationTemplateDto>
{
    public async ValueTask<CommunicationTemplateDto> Handle(GetCommunicationTemplateQuery query, CancellationToken ct)
    {
        var entity = await context.CommunicationTemplates
            .Where(x => x.Id == query.Id)
            .Select(x => new CommunicationTemplateDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CommunicationTemplate not found");
    }
}
