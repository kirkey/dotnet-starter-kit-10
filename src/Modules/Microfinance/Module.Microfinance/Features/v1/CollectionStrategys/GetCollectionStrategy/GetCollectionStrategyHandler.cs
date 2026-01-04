using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CollectionStrategys.GetCollectionStrategy;

public record GetCollectionStrategyQuery(Guid Id) : IQuery<CollectionStrategyDto>;

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
