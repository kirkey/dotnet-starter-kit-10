using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.GetCollectionStrategy;
using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys;

namespace FSH.Module.Microfinance.Features.v1.CollectionStrategys.GetCollectionStrategy;

public class GetCollectionStrategyHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollectionStrategyQuery, CollectionStrategyDto>
{
    public async ValueTask<CollectionStrategyDto> Handle(GetCollectionStrategyQuery query, CancellationToken ct)
    {
        var entity = await context.CollectionStrategys
            .Where(x => x.Id == query.Id)
            .Select(x => new CollectionStrategyDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CollectionStrategy not found");
    }
}
