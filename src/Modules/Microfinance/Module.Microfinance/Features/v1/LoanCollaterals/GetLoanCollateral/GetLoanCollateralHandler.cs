using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.LoanCollaterals;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanCollaterals.GetLoanCollateral;

public record GetLoanCollateralQuery(Guid Id) : IQuery<LoanCollateralDto>;

public class GetLoanCollateralHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanCollateralQuery, LoanCollateralDto>
{
    public async ValueTask<LoanCollateralDto> Handle(GetLoanCollateralQuery query, CancellationToken ct)
    {
        var entity = await context.LoanCollaterals
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanCollateralDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LoanCollateral not found");
    }
}
