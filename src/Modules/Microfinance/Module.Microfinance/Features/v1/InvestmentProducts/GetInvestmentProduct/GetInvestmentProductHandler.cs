using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.InvestmentProducts;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.InvestmentProducts.GetInvestmentProduct;

public record GetInvestmentProductQuery(Guid Id) : IQuery<InvestmentProductDto>;

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
