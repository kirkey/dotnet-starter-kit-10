using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.MfiConfigurations;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.MfiConfigurations.GetMfiConfiguration;

public record GetMfiConfigurationQuery(Guid Id) : IQuery<MfiConfigurationDto>;

public class GetMfiConfigurationHandler(MicrofinanceDbContext context) : IQueryHandler<GetMfiConfigurationQuery, MfiConfigurationDto>
{
    public async ValueTask<MfiConfigurationDto> Handle(GetMfiConfigurationQuery query, CancellationToken ct)
    {
        var entity = await context.MfiConfigurations
            .Where(x => x.Id == query.Id)
            .Select(x => new MfiConfigurationDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("MfiConfiguration not found");
    }
}
