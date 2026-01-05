using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollectionCases.GetCollectionCase;
using FSH.Module.Microfinance.Contracts.v1.CollectionCases;

namespace FSH.Module.Microfinance.Features.v1.CollectionCases.GetCollectionCase;

public class GetCollectionCaseHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollectionCaseQuery, CollectionCaseDto>
{
    public async ValueTask<CollectionCaseDto> Handle(GetCollectionCaseQuery query, CancellationToken ct)
    {
        var entity = await context.CollectionCases
            .Where(x => x.Id == query.Id)
            .Select(x => new CollectionCaseDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CollectionCase not found");
    }
}
