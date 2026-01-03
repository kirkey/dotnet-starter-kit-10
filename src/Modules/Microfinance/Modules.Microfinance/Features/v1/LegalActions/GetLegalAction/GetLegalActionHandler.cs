using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.LegalActions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LegalActions.GetLegalAction;

public record GetLegalActionQuery(Guid Id) : IQuery<LegalActionDto>;

public class GetLegalActionHandler(MicrofinanceDbContext context) : IQueryHandler<GetLegalActionQuery, LegalActionDto>
{
    public async ValueTask<LegalActionDto> Handle(GetLegalActionQuery query, CancellationToken ct)
    {
        var entity = await context.LegalActions
            .Where(x => x.Id == query.Id)
            .Select(x => new LegalActionDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LegalAction not found");
    }
}
