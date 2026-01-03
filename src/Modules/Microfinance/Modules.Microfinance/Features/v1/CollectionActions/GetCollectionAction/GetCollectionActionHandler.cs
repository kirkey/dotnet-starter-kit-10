using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.CollectionActions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollectionActions.GetCollectionAction;

public record GetCollectionActionQuery(Guid Id) : IQuery<CollectionActionDto>;

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
