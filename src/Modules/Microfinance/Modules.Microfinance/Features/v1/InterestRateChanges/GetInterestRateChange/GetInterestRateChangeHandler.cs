using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.InterestRateChanges;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InterestRateChanges.GetInterestRateChange;

public record GetInterestRateChangeQuery(Guid Id) : IQuery<InterestRateChangeDto>;

public class GetInterestRateChangeHandler(MicrofinanceDbContext context) : IQueryHandler<GetInterestRateChangeQuery, InterestRateChangeDto>
{
    public async ValueTask<InterestRateChangeDto> Handle(GetInterestRateChangeQuery query, CancellationToken ct)
    {
        var entity = await context.InterestRateChanges
            .Where(x => x.Id == query.Id)
            .Select(x => new InterestRateChangeDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("InterestRateChange not found");
    }
}
