using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ShareProducts.GetShareProduct;
using FSH.Module.Microfinance.Contracts.v1.ShareProducts;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.GetShareProduct;

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
