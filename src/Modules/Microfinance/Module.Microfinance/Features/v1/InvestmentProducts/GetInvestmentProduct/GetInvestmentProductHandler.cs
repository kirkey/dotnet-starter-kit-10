using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InvestmentProducts.GetInvestmentProduct;
using FSH.Module.Microfinance.Contracts.v1.InvestmentProducts;

namespace FSH.Module.Microfinance.Features.v1.InvestmentProducts.GetInvestmentProduct;

public class GetInvestmentProductHandler(MicrofinanceDbContext context) : IQueryHandler<GetInvestmentProductQuery, InvestmentProductDto>
{
    public async ValueTask<InvestmentProductDto> Handle(GetInvestmentProductQuery query, CancellationToken ct)
    {
        var entity = await context.InvestmentProducts
            .Where(x => x.Id == query.Id)
            .Select(x => new InvestmentProductDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("InvestmentProduct not found");
    }
}
