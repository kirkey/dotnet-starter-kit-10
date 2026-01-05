using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.RiskCategorys.GetRiskCategory;
using FSH.Module.Microfinance.Contracts.v1.RiskCategorys;

namespace FSH.Module.Microfinance.Features.v1.RiskCategorys.GetRiskCategory;

public class GetRiskCategoryHandler(MicrofinanceDbContext context) : IQueryHandler<GetRiskCategoryQuery, RiskCategoryDto>
{
    public async ValueTask<RiskCategoryDto> Handle(GetRiskCategoryQuery query, CancellationToken ct)
    {
        var entity = await context.RiskCategorys
            .Where(x => x.Id == query.Id)
            .Select(x => new RiskCategoryDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("RiskCategory not found");
    }
}
