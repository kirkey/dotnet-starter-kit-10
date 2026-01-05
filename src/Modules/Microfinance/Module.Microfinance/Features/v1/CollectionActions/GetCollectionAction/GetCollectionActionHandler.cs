using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollectionActions.GetCollectionAction;
using FSH.Module.Microfinance.Contracts.v1.CollectionActions;

namespace FSH.Module.Microfinance.Features.v1.CollectionActions.GetCollectionAction;

public class GetCollectionActionHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollectionActionQuery, CollectionActionDto>
{
    public async ValueTask<CollectionActionDto> Handle(GetCollectionActionQuery query, CancellationToken ct)
    {
        var entity = await context.CollectionActions
            .Where(x => x.Id == query.Id)
            .Select(x => new CollectionActionDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CollectionAction not found");
    }
}
