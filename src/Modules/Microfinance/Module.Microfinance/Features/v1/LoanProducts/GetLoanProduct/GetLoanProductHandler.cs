using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.LoanProducts;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanProducts.GetLoanProduct;

public record GetLoanProductQuery(Guid Id) : IQuery<LoanProductDto>;

public class GetLoanProductHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanProductQuery, LoanProductDto>
{
    public async ValueTask<LoanProductDto> Handle(GetLoanProductQuery query, CancellationToken ct)
    {
        var entity = await context.LoanProducts
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanProductDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LoanProduct not found");
    }
}
