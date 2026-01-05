using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.DebtSettlements.GetDebtSettlement;
using FSH.Module.Microfinance.Contracts.v1.DebtSettlements;

namespace FSH.Module.Microfinance.Features.v1.DebtSettlements.GetDebtSettlement;

public class GetDebtSettlementHandler(MicrofinanceDbContext context) : IQueryHandler<GetDebtSettlementQuery, DebtSettlementDto>
{
    public async ValueTask<DebtSettlementDto> Handle(GetDebtSettlementQuery query, CancellationToken ct)
    {
        var entity = await context.DebtSettlements
            .Where(x => x.Id == query.Id)
            .Select(x => new DebtSettlementDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("DebtSettlement not found");
    }
}
