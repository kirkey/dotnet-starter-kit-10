using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.ShareProducts;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.GetShareProduct;

public record GetShareProductQuery(Guid Id) : IQuery<ShareProductDto>;

public class GetShareProductHandler(MicrofinanceDbContext context) : IQueryHandler<GetShareProductQuery, ShareProductDto>
{
    public async ValueTask<ShareProductDto> Handle(GetShareProductQuery query, CancellationToken ct)
    {
        var entity = await context.ShareProducts
            .Where(x => x.Id == query.Id)
            .Select(x => new ShareProductDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("ShareProduct not found");
    }
}
