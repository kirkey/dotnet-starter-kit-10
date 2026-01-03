using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.FeeCharges;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.FeeCharges.GetFeeCharge;

public record GetFeeChargeQuery(Guid Id) : IQuery<FeeChargeDto>;

public class GetFeeChargeHandler(MicrofinanceDbContext context) : IQueryHandler<GetFeeChargeQuery, FeeChargeDto>
{
    public async ValueTask<FeeChargeDto> Handle(GetFeeChargeQuery query, CancellationToken ct)
    {
        var entity = await context.FeeCharges
            .Where(x => x.Id == query.Id)
            .Select(x => new FeeChargeDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("FeeCharge not found");
    }
}
