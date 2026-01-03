using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.FeeDefinitions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.FeeDefinitions.GetFeeDefinition;

public record GetFeeDefinitionQuery(Guid Id) : IQuery<FeeDefinitionDto>;

public class GetFeeDefinitionHandler(MicrofinanceDbContext context) : IQueryHandler<GetFeeDefinitionQuery, FeeDefinitionDto>
{
    public async ValueTask<FeeDefinitionDto> Handle(GetFeeDefinitionQuery query, CancellationToken ct)
    {
        var entity = await context.FeeDefinitions
            .Where(x => x.Id == query.Id)
            .Select(x => new FeeDefinitionDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("FeeDefinition not found");
    }
}
