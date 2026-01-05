using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LegalActions.GetLegalAction;
using FSH.Module.Microfinance.Contracts.v1.LegalActions;

namespace FSH.Module.Microfinance.Features.v1.LegalActions.GetLegalAction;

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
