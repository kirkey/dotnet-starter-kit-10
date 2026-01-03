using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.CollectionCases;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollectionCases.GetCollectionCase;

public record GetCollectionCaseQuery(Guid Id) : IQuery<CollectionCaseDto>;

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
