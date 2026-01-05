using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanRestructures.GetLoanRestructure;
using FSH.Module.Microfinance.Contracts.v1.LoanRestructures;

namespace FSH.Module.Microfinance.Features.v1.LoanRestructures.GetLoanRestructure;

public class GetLoanRestructureHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanRestructureQuery, LoanRestructureDto>
{
    public async ValueTask<LoanRestructureDto> Handle(GetLoanRestructureQuery query, CancellationToken ct)
    {
        var entity = await context.LoanRestructures
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanRestructureDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LoanRestructure not found");
    }
}
