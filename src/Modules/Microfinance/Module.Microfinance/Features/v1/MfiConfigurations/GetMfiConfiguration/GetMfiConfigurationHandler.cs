using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.MfiConfigurations.GetMfiConfiguration;
using FSH.Module.Microfinance.Contracts.v1.MfiConfigurations;

namespace FSH.Module.Microfinance.Features.v1.MfiConfigurations.GetMfiConfiguration;

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
