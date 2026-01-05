using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.FeeWaivers;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.FeeWaivers.GetFeeWaiver;

namespace FSH.Module.Microfinance.Features.v1.FeeWaivers.GetFeeWaiver;

public class GetFeeWaiverHandler(MicrofinanceDbContext context) : IQueryHandler<GetFeeWaiverQuery, FeeWaiverDto>
{
    public async ValueTask<FeeWaiverDto> Handle(GetFeeWaiverQuery query, CancellationToken ct)
    {
        var entity = await context.FeeWaivers
            .Where(x => x.Id == query.Id)
            .Select(x => new FeeWaiverDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("FeeWaiver not found");
    }
}
