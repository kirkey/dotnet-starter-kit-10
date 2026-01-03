using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.RiskCategorys;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.RiskCategorys.GetRiskCategory;

public record GetRiskCategoryQuery(Guid Id) : IQuery<RiskCategoryDto>;

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
