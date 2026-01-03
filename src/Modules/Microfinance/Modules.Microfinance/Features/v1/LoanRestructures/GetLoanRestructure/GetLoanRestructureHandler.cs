using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.LoanRestructures;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanRestructures.GetLoanRestructure;

public record GetLoanRestructureQuery(Guid Id) : IQuery<LoanRestructureDto>;

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
