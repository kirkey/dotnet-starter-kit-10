using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.GetInterestRateChange;
using FSH.Module.Microfinance.Contracts.v1.InterestRateChanges;

namespace FSH.Module.Microfinance.Features.v1.InterestRateChanges.GetInterestRateChange;

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
