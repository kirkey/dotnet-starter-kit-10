using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.SavingsProducts;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.SavingsProducts.GetSavingsProduct;

namespace FSH.Module.Microfinance.Features.v1.SavingsProducts.GetSavingsProduct;

public class GetSavingsProductHandler(MicrofinanceDbContext context) : IQueryHandler<GetSavingsProductQuery, SavingsProductDto>
{
    public async ValueTask<SavingsProductDto> Handle(GetSavingsProductQuery query, CancellationToken ct)
    {
        var entity = await context.SavingsProducts
            .Where(x => x.Id == query.Id)
            .Select(x => new SavingsProductDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("SavingsProduct not found");
    }
}
