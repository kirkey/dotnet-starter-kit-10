using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.FixedDeposits;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.FixedDeposits.GetFixedDeposit;

public record GetFixedDepositQuery(Guid Id) : IQuery<FixedDepositDto>;

public class GetFixedDepositHandler(MicrofinanceDbContext context) : IQueryHandler<GetFixedDepositQuery, FixedDepositDto>
{
    public async ValueTask<FixedDepositDto> Handle(GetFixedDepositQuery query, CancellationToken ct)
    {
        var entity = await context.FixedDeposits
            .Where(x => x.Id == query.Id)
            .Select(x => new FixedDepositDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("FixedDeposit not found");
    }
}
