using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.InsuranceProducts;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InsuranceProducts.GetInsuranceProduct;

public record GetInsuranceProductQuery(Guid Id) : IQuery<InsuranceProductDto>;

public class GetInsuranceProductHandler(MicrofinanceDbContext context) : IQueryHandler<GetInsuranceProductQuery, InsuranceProductDto>
{
    public async ValueTask<InsuranceProductDto> Handle(GetInsuranceProductQuery query, CancellationToken ct)
    {
        var entity = await context.InsuranceProducts
            .Where(x => x.Id == query.Id)
            .Select(x => new InsuranceProductDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("InsuranceProduct not found");
    }
}
